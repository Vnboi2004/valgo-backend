namespace VAlgo.JudgeWorker.Models
{
    public sealed class CodeTemplateForJudgeDto
    {
        public string Language { get; init; } = null!;
        public string UserTemplate { get; init; } = null!;
        public string JudgeTemplateHeader { get; init; } = null!;
        public string JudgeTemplateFooter { get; init; } = null!;

        // Ghép UserCode + JudgeTemplate
        public string GetFullCode(string userSolution)
            => $"{JudgeTemplateHeader}\n\n{userSolution}\n\n{JudgeTemplateFooter}";

    }
}