using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Domain.Aggregates;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Repositories
{
    public sealed class ContestRepository : IContestRepository
    {
        private readonly ContestsDbContext _dbContext;

        public ContestRepository(ContestsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task AddAsync(Contest contest, CancellationToken cancellationToken = default)
        {
            await _dbContext.Contests.AddAsync(contest, cancellationToken);
        }

        public async Task UpdateAsync(Contest contest, CancellationToken cancellationToken = default)
        {
            _dbContext.Contests.Update(contest);
            await Task.CompletedTask;
        }

        public async Task<Contest?> GetByIdAsync(ContestId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Contests
                .Include(x => x.Problems)
                .Include(x => x.Participants)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Contest>> GetContestsAsync(
            ContestStatus? status,
            ContestVisibility? visibility,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default
        )
        {
            var query = _dbContext.Contests.AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status.Value);

            if (visibility.HasValue)
                query = query.Where(x => x.Visibility == visibility.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Contest>(items, totalCount, page, pageSize);
        }
    }
}