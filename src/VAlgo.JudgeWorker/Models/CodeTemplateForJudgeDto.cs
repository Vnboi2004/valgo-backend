namespace VAlgo.JudgeWorker.Models
{
    public sealed class CodeTemplateForJudgeDto
    {
        public string Language { get; init; } = null!;
        public string UserTemplate { get; init; } = null!;
        public string JudgeTemplate { get; init; } = null!;

        // Ghép UserCode + JudgeTemplate
        public string GetFullCode(string userSolution)
            => $"{userSolution}\n\n{JudgeTemplate}";

    }
}