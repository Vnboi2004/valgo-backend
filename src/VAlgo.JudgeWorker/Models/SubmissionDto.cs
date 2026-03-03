namespace VAlgo.JudgeWorker.Models
{
    public sealed class SubmissionDto
    {
        public Guid SubmissionId { get; init; }
        public Guid UserId { get; init; }
        public Guid ProblemId { get; init; }
        public SubmissionStatus Status { get; init; }
        public string Language { get; init; } = null!;
        public string SourceCode { get; init; } = null!;
        public int RetryCount { get; init; }
    }
}