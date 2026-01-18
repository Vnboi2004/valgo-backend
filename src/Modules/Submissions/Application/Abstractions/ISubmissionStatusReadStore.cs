using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionStatus;

namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface ISubmissionStatusReadStore
    {
        Task<SubmissionStatusDto?> GetStatusAsync(Guid submissionId, CancellationToken cancellationToken);
    }
}