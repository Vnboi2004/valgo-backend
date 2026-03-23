using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.Modules.Identity.Infrastructure.Persistence;
using VAlgo.Modules.Submissions.Application.Abstractions;

namespace VAlgo.API.Services
{
    public sealed class UserReadService : IUserReadService
    {
        private readonly IdentityDbContext _dbContext;

        public UserReadService(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == UserId.From(userId));
        }
    }
}