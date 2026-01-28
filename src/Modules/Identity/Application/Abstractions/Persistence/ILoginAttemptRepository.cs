using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Application.Abstractions.Persistence
{
    public interface ILoginAttemptRepository
    {
        Task AddAsync(LoginAttempt attempt, CancellationToken cancellationToken = default);
        Task<int> CountFailedAttemptsAsync(UserId userId, DateTimeOffset since, CancellationToken cancellationToken = default);
    }
}