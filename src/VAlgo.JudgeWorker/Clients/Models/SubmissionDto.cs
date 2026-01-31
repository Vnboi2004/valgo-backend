namespace VAlgo.JudgeWorker.Clients.Models
{
    public sealed class SubmissionDto
    {
        public Guid Id { get; init; }

        public Guid UserId { get; init; }
        public Guid ProblemId { get; init; }

        public string Language { get; init; } = null!;
        public string SourceCode { get; init; } = null!;

        public int TimeLimitMs { get; init; }
        public int MemoryLimitKb { get; init; }

        public int RetryCount { get; init; }

        public SubmissionStatus Status { get; init; }
    }

    public enum SubmissionStatus
    {
        Created = 0,
        Queued = 1,
        Running = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5
    }
}