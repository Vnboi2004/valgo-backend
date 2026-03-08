using VAlgo.Modules.Discussions.Domain.Aggregates;
using VAlgo.Modules.Discussions.Domain.ValueObjects;

namespace VAlgo.Modules.Discussions.Application.Interfaces
{
    public interface IDiscussionRepository
    {
        Task AddAsync(Discussion discussion, CancellationToken cancellationToken = default);
        Task UpdateAsync(Discussion discussion, CancellationToken cancellationToken = default);
        Task DeleteAsync(Discussion discussion, CancellationToken cancellationToken = default);
        Task<Discussion?> GetByIdAsync(DiscussionId id, CancellationToken cancellationToken = default);
    }
}