using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly SubmissionsDbContext _dbContext;

        public UnitOfWork(SubmissionsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _dbContext.SaveChangesAsync(cancellationToken);

    }
}