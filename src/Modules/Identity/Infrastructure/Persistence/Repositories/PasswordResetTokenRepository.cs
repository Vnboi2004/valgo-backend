using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Domain.Aggregates;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence.Repositories
{
    public sealed class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private readonly IdentityDbContext _dbContext;

        public PasswordResetTokenRepository(IdentityDbContext dbContext)
            => _dbContext = dbContext;

        public async Task AddAsync(PasswordResetToken token, CancellationToken cancellationToken = default)
        {
            await _dbContext.PasswordResetTokens.AddAsync(token, cancellationToken);
        }

        public async Task<PasswordResetToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbContext.PasswordResetTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        }

    }
}