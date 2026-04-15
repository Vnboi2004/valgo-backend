using VAlgo.Modules.Contests.Application.Interfaces;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ContestsDbContext _dbContext;

        public UnitOfWork(ContestsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}