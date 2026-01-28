using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface IProblemRepository
    {
        Task AddAsync(Problem problem, CancellationToken cancellationToken = default);
        Task UpdateAsync(Problem problem, CancellationToken cancellationToken = default);
        Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task<Problem?> GetByIdAsync(ProblemId id, CancellationToken cancellationToken = default);
    }
}