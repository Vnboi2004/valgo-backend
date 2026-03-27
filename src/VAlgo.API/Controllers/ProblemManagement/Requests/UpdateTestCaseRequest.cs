using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record UpdateTestCaseRequest(
        string Input,
        string ExpectedOutput,
        OutputComparisonStrategy OutputComparisonStrategy,
        bool IsSample
    );
}