using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemHints
{
    public sealed record GetProblemHintsQuery(Guid ProblemId) : IRequest<IReadOnlyList<ProblemHintDto>>;
}