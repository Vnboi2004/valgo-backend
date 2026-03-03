using VAlgo.Modules.Submissions.Domain.Aggregates;
using VAlgo.Modules.Submissions.Domain.ValueObjects;

namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface ISubmissionRepository
    {
        Task AddAsync(Submission submission, CancellationToken cancellationToken = default);
        Task<Submission?> GetByIdAsync(SubmissionId id, CancellationToken cancellationToken = default);
    }
}