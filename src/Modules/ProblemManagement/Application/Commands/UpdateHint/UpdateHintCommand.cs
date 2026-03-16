using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateHint
{
    public sealed record UpdateHintCommand(
        Guid ProblemId,
        Guid HintId,
        string Content
    ) : IRequest<Unit>;
}