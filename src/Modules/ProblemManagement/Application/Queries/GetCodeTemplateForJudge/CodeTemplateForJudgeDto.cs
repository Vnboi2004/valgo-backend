namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplateForJudge
{
    public sealed class CodeTemplateForJudgeDto
    {
        public string Language { get; init; } = null!;
        public string UserTemplate { get; init; } = null!;
        public string JudgeTemplate { get; init; } = null!;

        // Judge ghép 2 phần lại để compile
        public string GetFullCode(string userSolution)
            => $"{userSolution}\n\n{JudgeTemplate}";

    }
}