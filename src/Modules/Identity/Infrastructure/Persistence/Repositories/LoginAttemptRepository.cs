using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.Enums;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence.Repositories
{
    public sealed class LoginAttemptRepository : ILoginAttemptRepository
    {
        private readonly IdentityDbContext _dbContext;

        public LoginAttemptRepository(IdentityDbContext dbContext)
            => _dbContext = dbContext;

        public async Task AddAsync(LoginAttempt attempt, CancellationToken cancellationToken = default)
        {
            await _dbContext.LoginAttempts.AddAsync(attempt, cancellationToken);
        }

        public async Task<int> CountFailedAttemptsAsync(UserId userId, DateTimeOffset since, CancellationToken cancellationToken = default)
        {
            return await _dbContext.LoginAttempts
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.Result == LoginResult.Failed && x.OccurredAt >= since)
                .CountAsync(cancellationToken);
        }
    }
}