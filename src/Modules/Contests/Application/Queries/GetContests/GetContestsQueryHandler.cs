using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Contests.Application.Queries.GetContests
{
    public sealed class GetContestsQueryHandler : IRequestHandler<GetContestsQuery, PagedResult<ContestListItemDto>>
    {
        private readonly IContestRepository _contestRepository;

        public GetContestsQueryHandler(IContestRepository contestRepository)
            => _contestRepository = contestRepository;

        public async Task<PagedResult<ContestListItemDto>> Handle(GetContestsQuery request, CancellationToken cancellationToken)
        {
            var contests = await _contestRepository.GetContestsAsync(request.Status, request.Visibility, request.Page, request.PageSize);

            var items = contests.Items.Select(x => new ContestListItemDto
            {
                Id = x.Id.Value,
                Title = x.Title,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Status = x.Status,
                Visibility = x.Visibility,
                ParticipantCount = x.Participants.Count
            }).ToList();

            return new PagedResult<ContestListItemDto>(items, contests.TotalCount, request.Page, request.PageSize);
        }
    }
}