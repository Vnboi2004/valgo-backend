using MediatR;
using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateTestCase
{
    public sealed record UpdateTestCaseCommand(
        Guid ProblemId,
        Guid TestCaseId,
        string Input,
        string ExpectedOutput,
        OutputComparisonStrategy OutputComparisonStrategy,
        bool IsSample
    ) : IRequest<Unit>;
}