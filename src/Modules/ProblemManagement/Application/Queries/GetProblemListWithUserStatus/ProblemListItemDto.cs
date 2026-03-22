using VAlgo.Modules.ProblemManagement.Application.Queries.GetUserProblemStatus;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemListWithUserStatus
{
    public sealed class ProblemListStatusItemDto
    {
        public Guid ProblemId { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string? ShortDescription { get; init; }
        public Difficulty Difficulty { get; init; }
        public ProblemStatus Status { get; init; }
        public int TotalSubmissions { get; init; }
        public int AcceptedSubmissions { get; init; }
        public double AcceptanceRate => TotalSubmissions == 0
            ? 0
            : Math.Round((double)AcceptedSubmissions / TotalSubmissions * 100, 1);
        public UserProblemStatus? UserStatus { get; init; }
    }
}