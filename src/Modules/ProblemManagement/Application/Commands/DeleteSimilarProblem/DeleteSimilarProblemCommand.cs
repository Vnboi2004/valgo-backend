using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.DeleteSimilarProblem
{
    public sealed record DeleteSimilarProblemCommand(
        Guid ProblemId,
        Guid SimilarProblemId
    ) : IRequest<Unit>;
}