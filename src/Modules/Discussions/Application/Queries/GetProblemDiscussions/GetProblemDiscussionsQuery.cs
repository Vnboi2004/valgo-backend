using MediatR;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Discussions.Application.Queries.GetProblemDiscussions
{
    public sealed record GetProblemDiscussionsQuery(
        Guid ProblemId,
        int Page,
        int PageSize
    ) : IRequest<PagedResult<ProblemDiscussionListItemDto>>;
}