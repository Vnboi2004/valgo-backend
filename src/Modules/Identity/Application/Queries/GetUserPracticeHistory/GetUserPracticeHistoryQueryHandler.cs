using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserPracticeHistory
{
    public sealed class GetUserPracticeHistoryQueryHandler : IRequestHandler<GetUserPracticeHistoryQuery, PagedResult<UserPracticeHistoryItemDto>>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserStatsReadService _userStatsReadService;

        public GetUserPracticeHistoryQueryHandler(IUserReadRepository userReadRepository, IUserStatsReadService userStatsReadService)
        {
            _userReadRepository = userReadRepository;
            _userStatsReadService = userStatsReadService;
        }

        public async Task<PagedResult<UserPracticeHistoryItemDto>> Handle(GetUserPracticeHistoryQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userReadRepository.GetUserIdByUsernameAsync(request.Username, cancellationToken);

            if (userId == null)
                throw new InvalidOperationException("User not found.");

            return await _userStatsReadService.GetUserPracticeHistoryAsync(userId.Value, request.Page, request.PageSize, cancellationToken);
        }
    }
}