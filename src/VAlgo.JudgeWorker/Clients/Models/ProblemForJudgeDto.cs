namespace VAlgo.JudgeWorker.Clients.Models
{
    public sealed class ProblemForJudgeDto
    {
        public Guid Id { get; init; }
        public int TimeLimitMs { get; init; }
        public int MemoryLimitKb { get; init; }
        public IReadOnlyList<TestCaseDto> TestCases { get; init; } = Array.Empty<TestCaseDto>();
    }
}
