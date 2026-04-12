using VAlgo.Modules.Contests.Application.DTOs;

namespace VAlgo.Modules.Contests.Application.Interfaces
{
    public interface ISubmissionReadService
    {
        Task<IReadOnlyList<SubmissionDto>> GetByContestIdAsync(Guid contestId, CancellationToken cancellationToken);
    }
}