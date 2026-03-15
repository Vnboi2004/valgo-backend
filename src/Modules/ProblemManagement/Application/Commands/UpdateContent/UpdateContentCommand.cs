using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands
{
    public sealed record UpdateContentCommand(
        Guid ProblemId,
        string Statement,
        string? Constraints,
        string? InputFormat,
        string? OutputFormat,
        string? FollowUp
    ) : IRequest<Unit>;
}