using VAlgo.Modules.ProblemManagement.Domain.Enums;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor
{
    public sealed class ProblemEditorTestCaseDto
    {
        public Guid TestCaseId { get; init; }
        public int Order { get; init; }
        public string Input { get; init; } = null!;
        public string ExpectedOutput { get; init; } = null!;
        public OutputComparisonStrategy ComparisonStrategy { get; init; }
        public bool IsSample { get; init; }
    }
}