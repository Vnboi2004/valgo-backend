using MediatR;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Discussions.Application.Queries.GetDiscussionComments
{
    public sealed record GetDiscussionCommentsQuery(Guid DiscussionId, int Page, int PageSize) : IRequest<PagedResult<DiscussionCommentDto>>;
}