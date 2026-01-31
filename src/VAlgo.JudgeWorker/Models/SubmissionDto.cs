namespace VAlgo.JudgeWorker.Models
{
    public sealed class SubmissionDto
    {
        public Guid Id { get; init; }
        public Guid ProblemId { get; init; }
        public string Language { get; init; } = default!;
        public string SourceCode { get; init; } = default!;
    }
}