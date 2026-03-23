namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetAllCodeTemplates
{
    public sealed class CodeTemplateEditorDto
    {
        public string Language { get; init; } = null!;
        public string UserTemplate { get; init; } = null!;
        public string JudgeTemplate { get; init; } = null!;
    }
}