namespace VAlgo.SharedKernel.CrossModule.Problems
{
    public interface ICodeTemplateReadService
    {
        Task<CodeTemplateForJudgeDto?> GetTemplateAsync(Guid problemId, string language, CancellationToken cancellationToken = default);
    }
}