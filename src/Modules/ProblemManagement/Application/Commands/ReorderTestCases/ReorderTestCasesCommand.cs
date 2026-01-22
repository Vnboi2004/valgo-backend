using MediatR;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ReorderTestCases
{
    public sealed record ReorderTestCasesCommand(
        Guid ProblemId,
        IReadOnlyList<Guid> OrderedTestCaseIds
    ) : ICommand<Unit>;
}