using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateProblemContent
{
    public sealed record UpdateProblemContentCommand(
        Guid ProblemId,
        string Statement,
        string? Constraints,
        string? InputFormat,
        string? OutputFormat,
        string? FollowUp
    ) : IRequest<Unit>;
}