using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail;

namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface ISubmissionDetailReadStore
    {
        Task<SubmissionDetailDto?> GetByIdAsync(Guid submissionId, CancellationToken cancellationToken);
    }
}