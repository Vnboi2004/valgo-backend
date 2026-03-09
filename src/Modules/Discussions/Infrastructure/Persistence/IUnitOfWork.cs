using VAlgo.Modules.Discussions.Application.Interfaces;

namespace VAlgo.Modules.Discussions.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly DiscussionsDbContext _dbContext;

        public UnitOfWork(DiscussionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}