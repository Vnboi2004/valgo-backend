using VAlgo.Modules.Submissions.Application.Queries.GetSubmissions;

namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface ISubmissionReadStore
    {
        Task<(IReadOnlyList<SubmissionListItemDto> Items, int TotalCount)> GetSubmissionsAsync(
            Guid? userId,
            Guid? problemId,
            int skip,
            int take,
            CancellationToken cancellationToken
        );
    }
}