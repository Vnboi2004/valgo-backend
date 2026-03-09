using MediatR;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Domain.ValueObjects;

namespace VAlgo.Modules.Discussions.Application.Queries.GetDiscussionDetail
{
    public sealed class GetDiscussionDetailQueryHandler : IRequestHandler<GetDiscussionDetailQuery, DiscussionDetailDto>
    {
        private readonly IDiscussionRepository _discussionRepository;

        public GetDiscussionDetailQueryHandler(
            IDiscussionRepository discussionRepository)
        {
            _discussionRepository = discussionRepository;
        }

        public async Task<DiscussionDetailDto> Handle(GetDiscussionDetailQuery request, CancellationToken cancellationToken)
        {
            var discussionId = DiscussionId.From(request.DiscussionId);

            var discussion = await _discussionRepository.GetByIdAsync(discussionId, cancellationToken);

            if (discussion == null)
                throw new InvalidOperationException("Discussion not found.");

            return new DiscussionDetailDto
            {
                Id = discussion.Id.Value,
                ProblemId = discussion.ProblemId,
                AuthorId = discussion.AuthorId,
                Title = discussion.Title,
                Content = discussion.Content,
                IsLocked = discussion.IsLocked,
                CommentCount = discussion.CommentCount,
                CreatedAt = discussion.CreatedAt,
                UpdatedAt = discussion.UpdatedAt
            };
        }
    }
}