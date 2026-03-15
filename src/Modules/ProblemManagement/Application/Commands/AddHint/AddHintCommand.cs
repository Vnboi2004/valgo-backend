using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AddHint
{
    public sealed record AddHintCommand(
        Guid ProblemId,
        string Content
    ) : IRequest<Unit>;
}