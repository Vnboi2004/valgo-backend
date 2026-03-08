using MediatR;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Domain.ValueObjects;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Discussions.Application.Queries.GetDiscussionComments
{
    public sealed class GetDiscussionCommentsQueryHandler : IRequestHandler<GetDiscussionCommentsQuery, PagedResult<DiscussionCommentDto>>
    {
        private readonly IDiscussionRepository _discussionRepository;

        public GetDiscussionCommentsQueryHandler(IDiscussionRepository discussionRepository)
        {
            _discussionRepository = discussionRepository;
        }

        public async Task<PagedResult<DiscussionCommentDto>> Handle(GetDiscussionCommentsQuery request, CancellationToken cancellationToken)
        {
            var discussionId = DiscussionId.From(request.DiscussionId);

            var discussion = await _discussionRepository.GetByIdAsync(
                discussionId,
                cancellationToken);

            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            var comments = discussion.Comments
                .OrderBy(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var items = comments.Select(c => new DiscussionCommentDto
            {
                Id = c.Id.Value,
                AuthorId = c.AuthorId,
                Content = c.Content,
                ParentCommentId = c.ParentCommentId?.Value,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return new PagedResult<DiscussionCommentDto>(
                items,
                discussion.CommentCount,
                request.Page,
                request.PageSize);
        }
    }
}