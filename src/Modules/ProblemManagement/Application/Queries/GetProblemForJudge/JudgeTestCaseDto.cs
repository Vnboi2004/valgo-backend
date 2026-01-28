using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge
{
    public sealed class JudgeTestCaseDto
    {
        public int Order { get; init; }
        public string Input { get; init; } = null!;
        public string ExpectedOutput { get; init; } = null!;
        public OutputComparisonStrategy ComparisonStrategy { get; init; }
    }
}