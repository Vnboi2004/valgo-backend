using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissions
{
    public sealed class SubmissionListItemDto
    {
        public Guid SubmissionId { get; init; }
        public Guid UserId { get; init; }
        public Guid ProblemId { get; init; }
        public string Language { get; init; } = null!;
        public SubmissionStatus Status { get; init; }
        public Verdict Verdict { get; init; }
        public int? PassedTestCases { get; init; }
        public int? TotalTestCases { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? FinishedAt { get; init; }
    }
}