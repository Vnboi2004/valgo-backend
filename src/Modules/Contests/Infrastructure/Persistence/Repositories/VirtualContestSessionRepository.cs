using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.Entities;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Repositories
{
    public sealed class VirtualContestSessionRepository : IVirtualContestSessionRepository
    {
        private readonly ContestsDbContext _dbContext;

        public VirtualContestSessionRepository(ContestsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(VirtualContestSession session, CancellationToken cancellationToken)
        {
            await _dbContext.VirtualContestSessions.AddAsync(session, cancellationToken);
        }

        public async Task<bool> ExistsAsync(Guid contestId, Guid userId)
        {
            return await _dbContext.VirtualContestSessions
                .AnyAsync(x => x.ContestId.Value == contestId && x.UserId == userId);
        }

        public async Task<VirtualContestSession?> GetAsync(Guid contestId, Guid userId)
        {
            return await _dbContext.VirtualContestSessions
                .FirstOrDefaultAsync(x => x.ContestId.Value == contestId && x.UserId == userId);
        }
    }
}