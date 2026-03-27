namespace VAlgo.SharedKernel.CrossModule.Problems
{
    public interface IProblemReadService
    {
        Task<ProblemForJudgeDto?> GetForJudgeAsync(Guid problemId, CancellationToken cancellationToken = default);
    }
}