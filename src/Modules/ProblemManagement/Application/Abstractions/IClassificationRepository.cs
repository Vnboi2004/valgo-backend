using VAlgo.Modules.ProblemClassification.Domain.Aggregates;
using VAlgo.Modules.ProblemClassification.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface IClassificationRepository
    {
        Task<Classification?> GetByIdAsync(ClassificationId classificationId, CancellationToken cancellationToken = default);
    }
}