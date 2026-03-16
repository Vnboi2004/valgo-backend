using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ReorderHints
{
    public sealed record ReorderHintsCommand(
        Guid ProblemId,
        IReadOnlyList<Guid> HintIds
    ) : IRequest<Unit>;
}