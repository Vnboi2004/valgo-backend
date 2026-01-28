using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Domain.Aggregates;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence.Repositories
{
    public sealed class EmailVerificationTokenRepository : IEmailVerificationTokenRepository
    {
        private readonly IdentityDbContext _dbContext;

        public EmailVerificationTokenRepository(IdentityDbContext dbContext)
            => _dbContext = dbContext;

        public async Task AddAsync(EmailVerificationToken token, CancellationToken cancellationToken = default)
        {
            await _dbContext.EmailVerificationTokens.AddAsync(token, cancellationToken);
        }

        public async Task RemoveAsync(EmailVerificationToken token, CancellationToken cancellationToken = default)
        {
            _dbContext.EmailVerificationTokens.Remove(token);
            await Task.CompletedTask;
        }

        public async Task<EmailVerificationToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            return await _dbContext.EmailVerificationTokens
                .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
        }

    }
}