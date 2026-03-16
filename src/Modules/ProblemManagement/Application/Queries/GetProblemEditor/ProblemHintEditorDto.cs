namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor
{
    public sealed class ProblemHintEditorDto
    {
        public Guid HintId { get; init; }
        public int Order { get; init; }
        public string Content { get; init; } = null!;
    }
}