using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence.Repositories
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _dbContext;

        public UserRepository(IdentityDbContext dbContext)
            => _dbContext = dbContext;

        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(user, cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _dbContext.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<bool> EmailExistsAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AnyAsync(x => x.Email == email, cancellationToken);
        }

        public async Task<bool> UsernameExistsAsync(Username username, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users
                .AnyAsync(x => x.Username == username, cancellationToken);
        }
    }
}