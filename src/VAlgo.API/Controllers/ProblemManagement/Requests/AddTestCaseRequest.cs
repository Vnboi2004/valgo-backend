using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.API.Controllers.ProblemManagement.Requests
{
    public sealed record AddTestCaseRequest(
        string Input,
        string ExpectedOutput,
        OutputComparisonStrategy OutputComparisonStrategy,
        bool IsSample
    );
}