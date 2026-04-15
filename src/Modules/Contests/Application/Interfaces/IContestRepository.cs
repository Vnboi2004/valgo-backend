using VAlgo.Modules.Contests.Application.Queries.GetContests;
using VAlgo.Modules.Contests.Domain.Aggregates;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Contests.Application.Interfaces
{
    public interface IContestRepository
    {
        Task AddAsync(Contest contest, CancellationToken cancellationToken = default);
        Task UpdateAsync(Contest contest, CancellationToken cancellationToken = default);
        Task<Contest?> GetByIdAsync(ContestId id, CancellationToken cancellationToken = default);
        Task<PagedResult<ContestListItemDto>> GetContestsAsync(
            ContestPhase? phase,
            ContestVisibility? visibility,
            int page,
            int pageSize,
            bool isAdmin,
            CancellationToken cancellationToken = default
        );
        Task<IReadOnlyList<Contest>> GetPublishedContestsToStartAsync(DateTime now, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Contest>> GetRunningContestsToFinishAsync(DateTime now, CancellationToken cancellationToken);
        Task<IReadOnlyList<Contest>> GetRunningContestsToFreezeAsync(DateTime now, CancellationToken cancellationToken);
    }
}