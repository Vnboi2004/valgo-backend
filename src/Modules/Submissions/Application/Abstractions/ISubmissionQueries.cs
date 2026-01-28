using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissions;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionStatus;

namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface ISubmissionQueries
    {
        Task<(IReadOnlyList<SubmissionListItemDto> Items, int TotalCount)> GetListAsync(
            Guid? userId,
            Guid? problemId,
            int skip,
            int take,
            CancellationToken cancellationToken
        );
        Task<SubmissionDetailDto?> GetDetailAsync(Guid submissionId, CancellationToken cancellationToken);
        Task<SubmissionStatusDto?> GetStatusAsync(Guid submissionId, CancellationToken cancellationToken);
        Task<IReadOnlyList<TestCaseResultDto>> GetTestCasesAsync(Guid submissionId, CancellationToken cancellationToken);
    }
}