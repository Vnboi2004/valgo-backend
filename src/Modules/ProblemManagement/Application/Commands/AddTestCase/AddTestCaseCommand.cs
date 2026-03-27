using MediatR;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AddTestCase
{
    public sealed record AddTestCaseCommand(
        Guid ProblemId,
        string Input,
        string ExpectedOutput,
        OutputComparisonStrategy OutputComparisonStrategy,
        bool IsSample
    ) : ICommand<Unit>;
}