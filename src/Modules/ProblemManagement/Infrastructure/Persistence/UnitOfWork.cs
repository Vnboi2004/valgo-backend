using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ProblemManagementDbContext _dbContext;

        public UnitOfWork(ProblemManagementDbContext dbContext) => _dbContext = dbContext;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}