using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.Modules.ProblemClassification.Domain.Enums;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;
using VAlgo.Modules.ProblemClassification.Infrastructure.Persistence;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Repositories
{
    public sealed class ClassificationRepository : IClassificationRepository
    {
        private readonly ClassificationDbContext _dbContext;

        public ClassificationRepository(ClassificationDbContext dbContext) => _dbContext = dbContext;

        // Fake
        public async Task<Classification?> GetByIdAsync(ClassificationId classificationId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Classifications.FirstOrDefaultAsync(x => x.Id == classificationId, cancellationToken);
        }
    }
}