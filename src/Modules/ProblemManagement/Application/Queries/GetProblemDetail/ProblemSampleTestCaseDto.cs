namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail
{
    public sealed class ProblemSampleTestCaseDto
    {
        public int Order { get; init; }
        public string Input { get; init; } = null!;
        public string ExpectedOutput { get; init; } = null!;
    }
}