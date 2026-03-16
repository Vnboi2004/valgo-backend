using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.DeleteHint
{
    public sealed record DeleteHintCommand(
        Guid ProblemId,
        Guid HintId
    ) : IRequest<Unit>;
}