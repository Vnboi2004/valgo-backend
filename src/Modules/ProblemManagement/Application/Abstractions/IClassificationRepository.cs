using VAlgo.Modules.ProblemClassification.Domain.Aggregates;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface IClassificationRepository
    {
        Task<Classification?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}