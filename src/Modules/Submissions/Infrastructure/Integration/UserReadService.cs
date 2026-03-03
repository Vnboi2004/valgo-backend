using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;

namespace VAlgo.Modules.Submissions.Infrastructure.Integration
{
    public sealed class UserReadService : IUserReadService
    {
        private readonly SubmissionsDbContext _dbContext;

        public UserReadService(SubmissionsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _dbContext.Submissions
                .AsNoTracking()
                .AnyAsync(x => x.UserId == userId);
        }
    }
}