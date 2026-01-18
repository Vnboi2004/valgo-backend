using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissions
{
    public record GetSubmissionsQuery(
        Guid? UserId,
        Guid? ProblemId,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<PagedResult<SubmissionListItemDto>>;
}