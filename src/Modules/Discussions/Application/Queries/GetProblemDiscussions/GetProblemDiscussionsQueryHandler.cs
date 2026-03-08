using MediatR;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Discussions.Application.Queries.GetProblemDiscussions
{
    public sealed class GetProblemDiscussionsQueryHandler : IRequestHandler<GetProblemDiscussionsQuery, PagedResult<ProblemDiscussionListItemDto>>
    {
        private readonly IDiscussionRepository _discussionRepository;

        public GetProblemDiscussionsQueryHandler(
            IDiscussionRepository discussionRepository)
        {
            _discussionRepository = discussionRepository;
        }

        public async Task<PagedResult<ProblemDiscussionListItemDto>> Handle(
            GetProblemDiscussionsQuery request,
            CancellationToken cancellationToken)
        {
            var discussions = await _discussionRepository
                .GetByProblemIdAsync(
                    request.ProblemId,
                    request.Page,
                    request.PageSize,
                    cancellationToken);

            var items = discussions.Items
                .Select(d => new ProblemDiscussionListItemDto
                {
                    Id = d.Id.Value,
                    Title = d.Title,
                    AuthorId = d.AuthorId,
                    IsLocked = d.IsLocked,
                    CommentCount = d.CommentCount,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .ToList();

            return new PagedResult<ProblemDiscussionListItemDto>(
                items,
                discussions.TotalCount,
                request.Page,
                request.PageSize);
        }
    }
}