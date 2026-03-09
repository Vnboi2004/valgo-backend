using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Domain.Aggregates;
using VAlgo.Modules.Discussions.Domain.ValueObjects;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Discussions.Infrastructure.Persistence.Repositories
{
    public sealed class DiscussionRepository : IDiscussionRepository
    {
        private readonly DiscussionsDbContext _dbContext;

        public DiscussionRepository(DiscussionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Discussion discussion, CancellationToken cancellationToken = default)
        {
            await _dbContext.Discussions.AddAsync(discussion, cancellationToken);
        }

        public async Task UpdateAsync(Discussion discussion, CancellationToken cancellationToken = default)
        {
            _dbContext.Discussions.Update(discussion);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Discussion discussion, CancellationToken cancellationToken = default)
        {
            _dbContext.Discussions.Remove(discussion);
            await Task.CompletedTask;
        }

        public async Task<Discussion?> GetByIdAsync(DiscussionId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Discussions
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<PagedResult<Discussion>> GetByProblemIdAsync(Guid problemId, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Discussions
                .Where(x => x.ProblemId == problemId)
                .OrderByDescending(x => x.CreatedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<Discussion>(items, totalCount, page, pageSize);
        }
    }
}