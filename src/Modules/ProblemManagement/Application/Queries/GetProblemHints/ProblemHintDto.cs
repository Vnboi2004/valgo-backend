namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemHints
{
    public sealed class ProblemHintDto
    {
        public int Order { get; init; }
        public string Content { get; init; } = null!;
    }
}