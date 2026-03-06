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
        Task<PagedResult<Contest>> GetContestsAsync(
            ContestStatus? status,
            ContestVisibility? visibility,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default
        );
    }
}