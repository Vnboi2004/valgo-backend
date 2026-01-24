using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.API.Controllers.Problems.Requests
{
    public sealed record AddTestCaseRequest(
        string Input,
        string ExpectedOutput,
        OutputComparisonStrategy OutputComparisonStrategy,
        bool IsSample
    );
}