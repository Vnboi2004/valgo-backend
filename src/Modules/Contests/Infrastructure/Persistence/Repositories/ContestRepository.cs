using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Queries.GetContests;
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
                    .ThenInclude(p => p.ProblemStats)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PagedResult<ContestListItemDto>> GetContestsAsync(
            ContestPhase? phase,
            ContestVisibility? visibility,
            int page,
            int pageSize,
            bool isAdmin,
            CancellationToken cancellationToken = default
        )
        {
            var query = _dbContext.Contests.AsNoTracking().AsQueryable();

            var now = DateTime.UtcNow;

            if (!isAdmin)
            {
                query = query.Where(x => x.Status != ContestStatus.Draft && x.Status != ContestStatus.Archived);
            }

            if (visibility.HasValue)
                query = query.Where(x => x.Visibility == visibility.Value);

            if (phase.HasValue)
            {
                switch (phase.Value)
                {
                    case ContestPhase.Upcoming:
                        query = query.Where(x =>
                            x.Status == ContestStatus.Published &&
                            x.StartTime > now
                        );
                        break;

                    case ContestPhase.Running:
                        query = query.Where(x =>
                            x.Status == ContestStatus.Running
                        );
                        break;

                    case ContestPhase.Past:
                        query = query.Where(x =>
                            x.Status == ContestStatus.Finished ||
                            x.Status == ContestStatus.Archived
                        );
                        break;
                }
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ContestListItemDto
                {
                    Id = x.Id.Value,
                    Title = x.Title,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Status = x.Status,
                    Visibility = x.Visibility,
                    ParticipantCount = x.Participants.Count(p => p.ContestId == x.Id)
                })
                .ToListAsync(cancellationToken);

            return new PagedResult<ContestListItemDto>(items, totalCount, page, pageSize);
        }

        public async Task<IReadOnlyList<Contest>> GetPublishedContestsToStartAsync(DateTime now, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Contests
                .Where(x => x.Status == ContestStatus.Published && x.StartTime <= now)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Contest>> GetRunningContestsToFinishAsync(DateTime now, CancellationToken cancellationToken)
        {
            return await _dbContext.Contests
                .Where(x => x.Status == ContestStatus.Running && x.EndTime <= now)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<Contest>> GetRunningContestsToFreezeAsync(DateTime now, CancellationToken cancellationToken)
        {
            return await _dbContext.Contests
                .Where(x =>
                    x.Status == ContestStatus.Running &&
                    x.FreezeAt != null &&
                    x.FreezeAt <= now &&
                    !x.IsLeaderboardFrozen)
                .ToListAsync(cancellationToken);
        }
    }
}