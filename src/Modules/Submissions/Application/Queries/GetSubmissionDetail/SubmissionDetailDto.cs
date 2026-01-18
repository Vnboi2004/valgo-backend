using VAlgo.Modules.Submissions.Domain.Enums;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail
{
    public sealed record SubmissionDetailDto
    {
        public Guid SubmissionId { get; init; }
        public Guid UserId { get; init; }
        public Guid ProblemId { get; init; }

        public string Language { get; init; } = null!;
        public string SourceCode { get; init; } = null!;

        public SubmissionStatus Status { get; init; }
        public Verdict Verdict { get; init; }

        public JudgeSummaryDto? Summary { get; init; }

        public IReadOnlyList<TestCaseResultDto>? TestCases { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime? StartedAt { get; init; }
        public DateTime? FinishedAt { get; init; }
    }
}