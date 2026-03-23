namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetAllCodeTemplates
{
    public sealed class CodeTemplateEditorDto
    {
        public string Language { get; init; } = null!;
        public string UserTemplate { get; init; } = null!;
        public string JudgeTemplateHeader { get; init; } = null!;
        public string JudgeTemplateFooter { get; init; } = null!;
    }
}