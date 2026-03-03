using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemClassification.Infrastructure.Persistence.Repositories
{
    public sealed class ClassificationRepository : IClassificationRepository
    {
        private readonly ClassificationDbContext _dbContext;

        public ClassificationRepository(ClassificationDbContext dbContext)
            => _dbContext = dbContext;

        public async Task AddAsync(Classification classification, CancellationToken cancellationToken = default)
        {
            await _dbContext.Classifications.AddAsync(classification, cancellationToken);
        }

        public async Task UpdateAsync(Classification classification, CancellationToken cancellationToken = default)
        {
            _dbContext.Classifications.Update(classification);
            await Task.CompletedTask;
        }

        public async Task<Classification?> GetByIdAsync(ClassificationId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Classifications.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Classifications.AnyAsync(x => x.Code == code, cancellationToken);
        }
    }
}