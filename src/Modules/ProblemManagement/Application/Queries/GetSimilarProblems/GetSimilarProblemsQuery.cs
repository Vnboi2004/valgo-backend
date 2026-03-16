using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetSimilarProblems
{
    public sealed record GetSimilarProblemsQuery(Guid ProblemId) : IRequest<IReadOnlyList<SimilarProblemDto>>;
}