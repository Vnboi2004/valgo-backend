using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Contests.Application.Queries.GetContests
{
    public sealed class GetContestsQueryHandler : IRequestHandler<GetContestsQuery, PagedResult<ContestListItemDto>>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetContestsQueryHandler(IContestRepository contestRepository, ICurrentUserService currentUserService)
        {
            _contestRepository = contestRepository;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<ContestListItemDto>> Handle(GetContestsQuery request, CancellationToken cancellationToken)
        {
            var isAdmin = _currentUserService.IsInRole("Admin");
            return await _contestRepository.GetContestsAsync(request.Phase, request.Visibility, request.Page, request.PageSize, isAdmin);
        }
    }
}