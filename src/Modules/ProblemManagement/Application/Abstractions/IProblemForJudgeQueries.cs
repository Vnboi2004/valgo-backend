using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface IProblemForJudgeQueries
    {
        Task<ProblemForJudgeDto?> GetAsync(Guid problemId, CancellationToken cancellationToken = default);
    }
}