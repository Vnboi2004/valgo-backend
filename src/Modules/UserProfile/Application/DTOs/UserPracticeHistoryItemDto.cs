using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed class UserPracticeHistoryItemDto
    {
        public Guid ProblemId { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public Difficulty Difficulty { get; init; }
        public DateTimeOffset LastSubmittedAt { get; init; }
        public Verdict LastVerdict { get; init; }
        public int SubmissionCount { get; init; }
    }


}