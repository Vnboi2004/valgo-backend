namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge
{
    public sealed class ProblemForJudgeDto
    {
        public Guid ProblemId { get; init; }

        public int TimeLimitMs { get; init; }
        public int MemoryLimitKb { get; init; }

        public IReadOnlyList<string> AllowedLanguages { get; init; } = [];
        public IReadOnlyList<JudgeTestCaseDto> TestCases { get; init; } = [];
    }
}