using VAlgo.Modules.Contests.Domain.Aggregates;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Interfaces
{
    public interface IContestRepository
    {
        Task AddAsync(Contest contest, CancellationToken cancellationToken = default);
        Task UpdateAsync(Contest contest, CancellationToken cancellationToken = default);
        Task<Contest?> GetByIdAsync(ContestId id, CancellationToken cancellationToken = default);
    }
}