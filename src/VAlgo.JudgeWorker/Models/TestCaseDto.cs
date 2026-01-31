namespace VAlgo.JudgeWorker.Clients
{
    public sealed class TestCaseDto
    {
        public Guid Id { get; init; }
        public string Input { get; init; } = default!;
        public string ExceptedOutput { get; init; } = default!;
        public string OutputComparisonStrategy { get; init; } = "Exact";
    }
}