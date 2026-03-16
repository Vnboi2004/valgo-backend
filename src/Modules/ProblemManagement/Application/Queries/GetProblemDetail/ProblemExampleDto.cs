namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail
{
    public sealed class ProblemExampleDto
    {
        public int Order { get; init; }
        public string Input { get; init; } = null!;
        public string Output { get; init; } = null!;
        public string? Explanation { get; init; }
    }
}