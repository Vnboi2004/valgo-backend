using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record AddTestCaseRequest(
        string Input,
        string ExpectedOutput,
        OutputComparisonStrategy OutputComparisonStrategy,
        bool IsSample
    );
}