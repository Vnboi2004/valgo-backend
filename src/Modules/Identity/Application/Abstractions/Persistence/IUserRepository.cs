using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Application.Abstractions.Persistence
{
    public interface IUserRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
        Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default);
        Task<bool> UsernameExistsAsync(Username username, CancellationToken cancellationToken = default);
    }
}