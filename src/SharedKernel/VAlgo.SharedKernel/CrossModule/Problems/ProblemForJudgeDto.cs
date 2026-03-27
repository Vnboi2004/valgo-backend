namespace VAlgo.SharedKernel.CrossModule.Problems
{
    public sealed class ProblemForJudgeDto
    {
        public int TimeLimitMs { get; set; }
        public int MemoryLimitKb { get; set; }
        public List<TestCaseDto> SampleTestCases { get; set; } = new();
    }

    public sealed class TestCaseDto
    {
        public int Order { get; set; }
        public string Input { get; set; } = "";
        public string ExpectedOutput { get; set; } = "";
    }
}