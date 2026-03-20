namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditorial
{
    public sealed class ProblemEditorialDto
    {
        public Guid ProblemId { get; init; }
        public string Editorial { get; init; } = null!;
    }
}