using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityDbContext _dbContext;

        public UnitOfWork(IdentityDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);
    }
}