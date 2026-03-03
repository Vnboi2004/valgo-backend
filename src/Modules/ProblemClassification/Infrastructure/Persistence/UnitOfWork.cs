using VAlgo.Modules.ProblemClassification.Application;

namespace VAlgo.Modules.ProblemClassification.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ClassificationDbContext _dbContext;

        public UnitOfWork(ClassificationDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}