using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VAlgo.BuildingBlocks.Sandbox.Abstractions;
using VAlgo.BuildingBlocks.Sandbox.Implementations;
using VAlgo.BuildingBlocks.Sandbox.Judging;
using VAlgo.BuildingBlocks.Sandbox.Models;
using VAlgo.JudgeWorker.Clients;
using VAlgo.JudgeWorker.Models;
using VAlgo.SharedKernel.CrossModule.Submissions;
namespace VAlgo.JudgeWorker.Workers;

public sealed class JudgeConsumer : BackgroundService
{
    private readonly ILogger<JudgeConsumer> _logger;
    private readonly VAlgoApiClient _apiClient;
    private readonly ISandboxRunner _sandbox;

    private IConnection? _connection;
    private IModel? _channel;

    private const string QueueName = "judge.submission";

    public JudgeConsumer(
        ILogger<JudgeConsumer> logger,
        VAlgoApiClient apiClient,
        ISandboxRunner sandbox)
    {
        _logger = logger;
        _apiClient = apiClient;
        _sandbox = sandbox;
    }

    // ============================================================
    // START
    // ============================================================
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "valgo",
            Password = "123456",
            DispatchConsumersAsync = true,
            AutomaticRecoveryEnabled = true,
            NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.BasicQos(0, 1, false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += OnMessageReceivedAsync;

        _channel.BasicConsume(
            queue: QueueName,
            autoAck: false,
            consumer: consumer);

        _logger.LogInformation("JudgeConsumer started");

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    // ============================================================
    // MAIN JUDGE LOGIC
    // ============================================================
    private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs ea)
    {
        if (_channel == null)
            return;

        JudgeJobMessage? job = null;
        string? workDir = null;

        try
        {
            job = JsonSerializer.Deserialize<JudgeJobMessage>(
                Encoding.UTF8.GetString(ea.Body.ToArray()),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (job == null)
                throw new InvalidOperationException("Invalid JudgeJobMessage");

            _logger.LogInformation("Judging Submission={SubmissionId}", job.SubmissionId);

            var submission = await _apiClient.GetSubmissionAsync(job.SubmissionId);
            var problem = await _apiClient.GetProblemForJudgeAsync(job.ProblemId);

            var codeTemplate = await _apiClient.GetCodeTemplateForJudgeAsync(job.ProblemId, submission.Language);

            var fullCode = codeTemplate.GetFullCode(submission.SourceCode);
            _logger.LogInformation("Full code to compile:\n{Fullcode}:", fullCode);
            _logger.LogInformation("Language: {Lang}", submission.Language);
            _logger.LogInformation("Header:\n{Header}", codeTemplate?.JudgeTemplateHeader);
            _logger.LogInformation("User:\n{User}", submission.SourceCode);
            _logger.LogInformation("Footer:\n{Footer}", codeTemplate?.JudgeTemplateFooter);

            await _apiClient.StartSubmissionAsync(submission.SubmissionId);

            var language = SandboxLanguageRegistry.Resolve(submission.Language);

            _logger.LogInformation("Resolved DockerImage: {Image}", language.DockerImage);

            // ========================================================
            // 1. COMPILE
            // ========================================================
            var compile = await _sandbox.CompileAsync(
                new SandboxCompileRequest(fullCode, language));

            if (!compile.Success)
            {
                await _apiClient.CompleteSubmissionAsync(
                    submission.SubmissionId,
                    new CompleteSubmissionRequest(
                        problem.TestCases.Count,
                        0,
                        0,
                        0,
                        Verdict.CompileError,
                        new List<SubmissionTestCaseResult>()
                    ));

                _channel.BasicAck(ea.DeliveryTag, false);
                return;
            }

            workDir = compile.WorkDir;

            // ========================================================
            // 2. RUN TEST CASES
            // ========================================================
            int passed = 0;
            int maxTime = 0;
            int maxMemory = 0;
            var finalVerdict = Verdict.Accepted;

            var testCaseResults = new List<SubmissionTestCaseResult>();

            foreach (var tc in problem.TestCases.OrderBy(x => x.Order))
            {
                var run = await _sandbox.RunAsync(
                    new SandboxRunRequest(
                        fullCode,
                        tc.Input,
                        language,
                        problem.TimeLimitMs,
                        problem.MemoryLimitKb),
                    workDir);

                _logger.LogInformation(
                    "RUN RESULT: time={time}, memory={memory}, verdict={verdict}",
                    run.TimeMs,
                    run.MemoryKb,
                    run.Verdict
                );

                maxTime = Math.Max(maxTime, run.TimeMs);
                maxMemory = Math.Max(maxMemory, run.MemoryKb);

                Verdict caseVerdict;

                if (run.Verdict != Verdict.Accepted)
                {
                    caseVerdict = run.Verdict;
                    finalVerdict = run.Verdict;
                }
                else if (!OutputComparer.Equals(run.Stdout, tc.ExpectedOutput))
                {
                    caseVerdict = Verdict.WrongAnswer;
                    finalVerdict = Verdict.WrongAnswer;
                }
                else
                {
                    caseVerdict = Verdict.Accepted;
                    passed++;
                }

                testCaseResults.Add(new SubmissionTestCaseResult(
                    tc.Order,
                    caseVerdict,
                    run.TimeMs,
                    run.MemoryKb,
                    run.Stdout
                ));
            }

            _logger.LogInformation(
                "FINAL SUMMARY: maxTime={time}, maxMemory={memory}",
                maxTime,
                maxMemory
            );


            // ========================================================
            // 3. COMPLETE
            // ========================================================
            await _apiClient.CompleteSubmissionAsync(
                submission.SubmissionId,
                new CompleteSubmissionRequest(
                    testCaseResults.Count,
                    passed,
                    maxTime,
                    maxMemory,
                    finalVerdict,
                    testCaseResults
                ));

            _channel.BasicAck(ea.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Judge failed");

            if (job != null)
            {
                try
                {
                    await _apiClient.FailSubmissionAsync(job.SubmissionId, new FailSubmissionRequest(SubmissionFailureReason.InternalJudgeError));
                }
                catch { }
            }

            _channel.BasicAck(ea.DeliveryTag, false);
        }
        finally
        {
            if (workDir != null)
                DockerSandboxRunner.Cleanup(workDir);
        }
    }

    // ============================================================
    // STOP
    // ============================================================
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();
        return base.StopAsync(cancellationToken);
    }
}
