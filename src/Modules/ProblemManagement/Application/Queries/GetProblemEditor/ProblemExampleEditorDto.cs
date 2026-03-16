namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor
{
    public sealed class ProblemExampleEditorDto
    {
        public Guid ExampleId { get; init; }
        public int Order { get; init; }
        public string Input { get; init; } = null!;
        public string Output { get; init; } = null!;
        public string? Explanation { get; init; }

    }
}