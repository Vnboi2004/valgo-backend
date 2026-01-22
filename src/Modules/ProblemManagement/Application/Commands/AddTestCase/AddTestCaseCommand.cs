using MediatR;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;

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