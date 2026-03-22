using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemListWithUserStatus
{
    public sealed class ProblemListRawDto
    {
        public Guid ProblemId { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public string? ShortDescription { get; init; }
        public Difficulty Difficulty { get; init; }
        public ProblemStatus Status { get; init; }
        public int TotalSubmissions { get; init; }
        public int AcceptedSubmissions { get; init; }
    }
}