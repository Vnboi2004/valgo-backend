using VAlgo.JudgeWorker.Clients;

namespace VAlgo.JudgeWorker.Models
{
    public sealed class ProblemJudgeDto
    {
        public Guid Id { get; init; }
        public int TimeLimitMs { get; init; }
        public int MemoryLimitKb { get; init; }
        public IReadOnlyList<TestCaseDto> TestCases { get; init; } = [];
    }
}