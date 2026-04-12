using VAlgo.Modules.Contests.Domain.Entities;

namespace VAlgo.Modules.Contests.Application.Interfaces
{
    public interface IVirtualContestSessionRepository
    {
        Task AddAsync(VirtualContestSession session, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid contestId, Guid userId);
        Task<VirtualContestSession?> GetAsync(Guid contestId, Guid userId);
    }
}