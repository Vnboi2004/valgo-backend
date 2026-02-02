
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ProblemManagementDbContext _dbContext;

        public UnitOfWork(ProblemManagementDbContext dbContext)
        {
            Console.WriteLine($"[UnitOfWork] DbContext: {dbContext.GetHashCode()}");
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}