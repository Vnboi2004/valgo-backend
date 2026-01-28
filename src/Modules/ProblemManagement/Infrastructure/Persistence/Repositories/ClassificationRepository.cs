using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.Modules.ProblemClassification.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Repositories
{
    public sealed class ClassificationRepository : IClassificationRepository
    {
        private readonly ProblemManagementDbContext _dbContext;

        public ClassificationRepository(ProblemManagementDbContext dbContext) => _dbContext = dbContext;

        // Fake
        public async Task<Classification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var temp = Classification.Create("", "", ClassificationType.Tag);
            return temp;
        }
    }
}