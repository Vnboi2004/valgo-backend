using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemClassification.Application.Abstractions
{
    public interface IClassificationRepository
    {
        Task AddAsync(Classification classification, CancellationToken cancellationToken = default);
        Task UpdateAsync(Classification classification, CancellationToken cancellationToken = default);
        Task<Classification?> GetByIdAsync(ClassificationId id, CancellationToken cancellationToken = default);
        Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    }
}