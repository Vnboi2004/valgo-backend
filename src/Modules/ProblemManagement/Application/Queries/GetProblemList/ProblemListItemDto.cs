using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList
{
    public sealed record ProblemListItemDto
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public Difficulty Difficulty { get; init; }
        public ProblemStatus Status { get; init; }
        public double AcceptanceRate { get; init; }
    }

    public enum ProblemSortBy
    {
        Code,
        Title,
        Difficulty
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }
}