using VAlgo.Modules.Submissions.Domain.Entities;
using VAlgo.Modules.Submissions.Domain.Enums;
using VAlgo.Modules.Submissions.Domain.Events;
using VAlgo.Modules.Submissions.Domain.Exceptions;
using VAlgo.Modules.Submissions.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Submissions.Domain.Aggregates
{
    public sealed class Submission : AggregateRoot<SubmissionId>
    {
        public Guid UserId { get; private set; }
        public Guid ProblemId { get; private set; }
        public Language Language { get; private set; } = null!;
        public string SourceCode { get; private set; } = null!;
        public SourceCodeHash SourceCodeHash { get; private set; } = null!;
        public int TimeLimitMs { get; private set; }
        public int MemoryLimitKb { get; private set; }
        public int RetryCount { get; private set; }
        public SubmissionStatus Status { get; private set; }
        public Verdict Verdict { get; private set; }
        public JudgeSummary? JudgeSummary { get; private set; }
        public SubmissionFailureReason? FailureReason { get; private set; }
        public string? WorkerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? QueuedAt { get; private set; }
        public DateTime? StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        private readonly List<TestCaseResult> _testCaseResults = new();
        public IReadOnlyCollection<TestCaseResult> TestCaseResults => _testCaseResults;

        private Submission() { }

        private Submission(
            SubmissionId id,
            Guid userId,
            Guid problemId,
            Language language,
            string sourceCode,
            int timeLimitMs,
            int memoryLimitKb,
            DateTime now) : base(id)
        {
            Guard.AgainstNullOrEmpty(sourceCode, nameof(sourceCode));

            UserId = userId;
            ProblemId = problemId;
            Language = language;
            SourceCode = sourceCode;
            TimeLimitMs = timeLimitMs;
            MemoryLimitKb = memoryLimitKb;
            SourceCodeHash = SourceCodeHash.From(sourceCode);
            Status = SubmissionStatus.Created;
            Verdict = Verdict.Node;
            CreatedAt = now;
        }

        public static Submission Create(
            Guid userId,
            Guid problemId,
            Language language,
            string sourceCode,
            int timeLimitMs,
            int memoryLimitKb,
            DateTime now
        )
        {
            var submission = new Submission(SubmissionId.New(), userId, problemId, language, sourceCode, timeLimitMs, memoryLimitKb, now);

            submission.AddDomainEvent(new SubmissionCreatedDomainEvent(submission.Id.Value, now));

            return submission;
        }

        public void Enqueue(DateTime now)
        {
            EnsureStatus(SubmissionStatus.Created);

            Status = SubmissionStatus.Queued;
            QueuedAt = now;

            AddDomainEvent(new SubmissionEnqueuedDomainEvent(Id.Value, now));
        }

        public void StartRunning(DateTime now)
        {
            EnsureStatus(SubmissionStatus.Queued);

            Status = SubmissionStatus.Running;
            StartedAt = now;
        }

        public void Complete(Verdict verdict, JudgeSummary judgeSummary, DateTime now)
        {
            EnsureStatus(SubmissionStatus.Running);

            if (verdict == Verdict.Node)
                throw new SubmissionDomainException("Verdict cannot be None when completing submission");

            Status = SubmissionStatus.Completed;
            Verdict = verdict;
            JudgeSummary = judgeSummary;
            FinishedAt = now;

            AddDomainEvent(new SubmissionCompletedDomainEvent(Id.Value, verdict, now));
        }

        public void Fail(SubmissionFailureReason reason, DateTime now)
        {
            EnsureNotTerminal();

            Status = SubmissionStatus.Failed;
            Verdict = Verdict.SystemError;
            FailureReason = reason;
            FinishedAt = now;

            AddDomainEvent(new SubmissionFailedDomainEvent(Id.Value, reason, now));
        }

        public void Cancel(DateTime now)
        {
            EnsureNotTerminal();

            Status = SubmissionStatus.Cancelled;
            FinishedAt = now;
        }

        public void AddTestCaseResult(SubmissionId submissionId, int index, Verdict verdict, int timeMs, int memoryKb, string? output)
        {
            EnsureStatus(SubmissionStatus.Running);
            var testCaseResult = TestCaseResult.Create(submissionId, index, verdict, timeMs, memoryKb, output);
            _testCaseResults.Add(testCaseResult);
        }

        private void EnsureStatus(SubmissionStatus expected)
        {
            if (Status != expected)
                throw new SubmissionDomainException($"Invalid state transition from {Status} to {expected}");

        }

        private void EnsureNotTerminal()
        {
            if (Status is SubmissionStatus.Completed or SubmissionStatus.Failed or SubmissionStatus.Cancelled)
                throw new SubmissionDomainException($"Submission already in terminal state: {Status}");
        }

        public void IncrementRetry()
        {
            RetryCount++;
        }

        public void AssignWorker(string workerId)
        {
            EnsureStatus(SubmissionStatus.Queued);
            WorkerId = workerId;
        }
    }
}