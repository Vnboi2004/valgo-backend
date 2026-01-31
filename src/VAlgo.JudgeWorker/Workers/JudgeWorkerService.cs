using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VAlgo.JudgeWorker.Clients;
using VAlgo.JudgeWorker.Clients.Models;
using VAlgo.JudgeWorker.Common;
using VAlgo.JudgeWorker.Messaging;
using VAlgo.JudgeWorker.Orchestration;


namespace VAlgo.JudgeWorker.Workers
{
    public sealed class JudgeWorkerService : BackgroundService
    {
        private readonly IJobQueue _jobQueue;
        private readonly SubmissionsClient _submissionsClient;
        private readonly ProblemsClient _problemsClient;
        private readonly JudgeOrchestrator _judgeOrchestrator;
        private readonly WorkerIdentity _workerIdentity;
        private readonly JudgeWorkerOptions _judgeWorkerOptions;
        private readonly ILogger<JudgeWorkerService> _logger;

        public JudgeWorkerService(
            IJobQueue jobQueue,
            SubmissionsClient submissionsClient,
            ProblemsClient problemsClient,
            JudgeOrchestrator judgeOrchestrator,
            WorkerIdentity workerIdentity,
            JudgeWorkerOptions judgeWorkerOptions,
            ILogger<JudgeWorkerService> logger
        )
        {
            _jobQueue = jobQueue;
            _submissionsClient = submissionsClient;
            _problemsClient = problemsClient;
            _judgeOrchestrator = judgeOrchestrator;
            _workerIdentity = workerIdentity;
            _judgeWorkerOptions = judgeWorkerOptions;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "JudgeWorker {WorkerId} started",
                _workerIdentity.WorkerId
            );

            while (!stoppingToken.IsCancellationRequested)
            {
                SubmissionJob job;

                try
                {
                    job = await _jobQueue.DequeueAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to dequeue submission job");
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                    continue;
                }

                await HandleJobAsync(job, stoppingToken);
            }

            _logger.LogInformation("JudgeWorker {WorkerId} stopped", _workerIdentity.WorkerId);
        }

        private async Task HandleJobAsync(SubmissionJob job, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Worker {WorkerId} picked submissions {SubmissionId}",
                _workerIdentity.WorkerId,
                job.SubmissionId
            );

            SubmissionDto submission;

            try
            {
                submission = await _submissionsClient.GetSubmissionAsync(job.SubmissionId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch submission {SubmissionId}", job.SubmissionId);
                return;
            }

            if (submission.Status == SubmissionStatus.Completed || submission.Status == SubmissionStatus.Cancelled)
            {
                _logger.LogWarning(
                    "Submission {SubmissionId} already finished (Status={Status})",
                    submission.Id,
                    submission.Status
                );

                return;
            }

            if (submission.RetryCount > _judgeWorkerOptions.MaxRetryCount)
            {
                _logger.LogWarning(
                    "Submission {SubmissionId} exceeded max retry count",
                    submission.Id
                );

                await _submissionsClient.FailAsync(submission.Id, "Retry limit exceeded", cancellationToken);

                return;
            }

            ProblemForJudgeDto problem;

            try
            {
                problem = await _problemsClient.GetProblemForJudgeAsync(submission.ProblemId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to fetch problem {ProblemId} for submission {SubmissionId}",
                    submission.ProblemId,
                    submission.Id
                );

                await _submissionsClient.FailAsync(submission.Id, "Failed to load problem", cancellationToken);

                return;
            }

            try
            {

            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Submission {SubmissionId} cancelled", submission.Id);

                await _submissionsClient.CancelAsync(submission.Id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled error while judging submission {SubmissionId}",
                    submission.Id
                );

                await _submissionsClient.FailAsync(submission.Id, ex.Message, cancellationToken);
            }
        }
    }
}