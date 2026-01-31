namespace VAlgo.JudgeWorker.Clients.Models
{
    public sealed class TestCaseDto
    {
        public int Index { get; init; }

        public string Input { get; init; } = null!;
        public string ExpectedOutput { get; init; } = null!;

        public OutputComparisonStrategy ComparisonStrategy { get; init; }

        public bool IsSample { get; init; }
    }

    public enum OutputComparisonStrategy
    {
        Exact = 1,
        IgnoreWhitespace = 2,
        FloatingPoint = 3
    }
}
