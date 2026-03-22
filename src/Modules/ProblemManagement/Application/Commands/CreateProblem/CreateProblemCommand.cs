using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.CreateProblem
{
    public sealed record CreateProblemCommand(
        string Code,
        string Title,
        string Statement,
        string? ShortDescription,
        Difficulty Difficulty,
        int TimeLimitMs,
        int MemoryLimitKb
    ) : ICommand<Guid>;
}