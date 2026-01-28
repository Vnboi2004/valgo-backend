using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence.Repositories
{
    public sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IdentityDbContext _dbContext;

        public RefreshTokenRepository(IdentityDbContext dbContext)
            => _dbContext = dbContext;

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }

        public async Task RemoveAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
        {
            _dbContext.RefreshTokens.Remove(refreshToken);
            await Task.CompletedTask;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        }

        public async Task RevokeAllAsync(UserId userId, CancellationToken cancellationToken = default)
        {

        }
    }
}