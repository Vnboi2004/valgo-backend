using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail;

namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface IJudgeResultReadStore
    {
        Task<IReadOnlyList<TestCaseResultDto>> GetTestCasesAsync(Guid submissionId, CancellationToken cancellationToken);
    }
}