using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateConstraints
{
    public sealed record UpdateConstraintsCommand(
        Guid ProblemId,
        int TimeLimitMs,
        int MemoryLimitKb
    ) : ICommand<Unit>;
}