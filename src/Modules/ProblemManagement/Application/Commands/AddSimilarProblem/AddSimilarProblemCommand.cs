using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AddSimilarProblem
{
    public sealed record AddSimilarProblemCommand(
        Guid ProblemId,
        Guid SimilarProblemId
    ) : IRequest<Unit>;
}