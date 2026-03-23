namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplateForJudge
{
    public sealed class CodeTemplateForJudgeDto
    {
        public string Language { get; init; } = null!;
        public string UserTemplate { get; init; } = null!;
        public string JudgeTemplateHeader { get; init; } = null!;
        public string JudgeTemplateFooter { get; init; } = null!;

        // Judge ghép 2 phần lại để compile
        public string GetFullCode(string userSolution)
            => $"{JudgeTemplateHeader}\n\n{userSolution}\n\n{JudgeTemplateFooter}";

    }
}