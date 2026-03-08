using MediatR;

namespace VAlgo.Modules.Discussions.Application.Queries.GetDiscussionDetail
{
    public sealed record GetDiscussionDetailQuery(Guid DiscussionId) : IRequest<DiscussionDetailDto>;
}