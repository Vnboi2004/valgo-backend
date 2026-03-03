using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using VAlgo.JudgeWorker.Clients;
using VAlgo.JudgeWorker.Judging;
using VAlgo.JudgeWorker.Models;
using VAlgo.JudgeWorker.Sandbox;

namespace VAlgo.JudgeWorker.Workers;

public sealed class JudgeConsumer : BackgroundService
{
    private readonly ILogger<JudgeConsumer> _logger;
    private readonly VAlgoApiClient _apiClient;
    private readonly DockerSandboxRunner _sandbox;

    private IConnection? _connection;
    private IModel? _channel;

    private const string QueueName = "judge.submission";

    public JudgeConsumer(
        ILogger<JudgeConsumer> logger,
        VAlgoApiClient apiClient,
        DockerSandboxRunner sandbox)
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
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true
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

            await _apiClient.StartSubmissionAsync(submission.SubmissionId);

            var language = SandboxLanguageRegistry.Resolve(submission.Language);

            // ========================================================
            // 1️⃣ COMPILE
            // ========================================================
            var compile = await _sandbox.CompileAsync(
                new SandboxCompileRequest(submission.SourceCode, language));

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
            // 2️⃣ RUN TEST CASES
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
                        submission.SourceCode,
                        tc.Input,
                        language,
                        problem.TimeLimitMs,
                        problem.MemoryLimitKb),
                    workDir);

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
                    caseVerdict,          // 🔥 dùng Verdict enum
                    run.TimeMs,
                    run.MemoryKb,
                    run.Stdout            // 🔥 gửi output luôn
                ));

                if (caseVerdict != Verdict.Accepted)
                    break;
            }


            // ========================================================
            // 3️⃣ COMPLETE
            // ========================================================
            await _apiClient.CompleteSubmissionAsync(
                submission.SubmissionId,
                new CompleteSubmissionRequest(
                    problem.TestCases.Count,
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
                    await _apiClient.FailSubmissionAsync(
                        job.SubmissionId,
                        new FailSubmissionRequest(
                            SubmissionFailureReason.InternalJudgeError));
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
