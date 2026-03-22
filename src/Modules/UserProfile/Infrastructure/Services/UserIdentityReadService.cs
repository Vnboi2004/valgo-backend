using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.Modules.Identity.Infrastructure.Persistence;
using VAlgo.Modules.UserProfile.Application.Abstractions;

namespace VAlgo.Modules.UserProfile.Infrastructure.Services
{
    public sealed class UserIdentityReadService : IUserIdentityReadService
    {
        private readonly IdentityDbContext _identityDbContext;

        public UserIdentityReadService(IdentityDbContext identityDbContext)
        {
            _identityDbContext = identityDbContext;
        }

        public async Task<Guid?> GetUserIdByUsernameAsync(
            string username,
            CancellationToken cancellationToken)
        {
            var user = await _identityDbContext.Users
                .AsNoTracking()
                .Where(u => u.Username.Value == username)
                .Select(u => new { u.Id })
                .FirstOrDefaultAsync(cancellationToken);

            return user?.Id.Value;
        }

        public async Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _identityDbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Id == UserId.From(userId), cancellationToken);
        }
    }
}