using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetSimilarProblems
{
    public sealed class SimilarProblemDto
    {
        public Guid ProblemId { get; init; }

        public string Code { get; init; } = null!;

        public string Title { get; init; } = null!;

        public Difficulty Difficulty { get; init; }
    }
}