namespace VAlgo.JudgeWorker.Models
{
    public sealed class ProblemForJudgeDto
    {
        public Guid Id { get; init; }
        public int TimeLimitMs { get; init; }
        public int MemoryLimitKb { get; init; }
        public IReadOnlyList<string> AllowedLanguages { get; init; } = [];
        public IReadOnlyList<ProblemTestCaseDto> TestCases { get; init; } = [];
    }

    public sealed class ProblemTestCaseDto
    {
        public int Order { get; init; }
        public string Input { get; init; } = null!;
        public string ExpectedOutput { get; init; } = null!;
        public OutputComparisonStrategy comparisonStrategy { get; init; }
    }

    public enum OutputComparisonStrategy
    {
        Exact = 1,
        IgnoreWhitespace = 2,
        FloatingPoint = 3
    }
}