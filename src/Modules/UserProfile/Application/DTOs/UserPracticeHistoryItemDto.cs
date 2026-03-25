using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.CrossModule.Submissions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed class UserPracticeHistoryDto
    {
        public PagedResult<UserPracticeHistoryItemDto> History { get; init; } = null!;
        public UserPracticeHistorySummaryDto Summary { get; init; } = new();
    }

    public sealed class UserPracticeHistoryItemDto
    {
        public Guid ProblemId { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public Difficulty Difficulty { get; init; }
        public DateTimeOffset LastSubmittedAt { get; init; }
        public Verdict LastVerdict { get; init; }
        public int SubmissionCount { get; init; }
        public IReadOnlyList<UserSubmissionDetailDto> Submissions { get; init; } = [];
    }

    public sealed class UserPracticeHistorySummaryDto
    {
        public int TotalProblems { get; init; }
        public int EasySolved { get; init; }
        public int MediumSolved { get; init; }
        public int HardSolved { get; init; }
        public int TotalSubmissions { get; init; }
        public double AcceptanceRate { get; init; }
    }
}