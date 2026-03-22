using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserRecentAc
{
    public sealed class GetUserRecentAcQueryHandler : IRequestHandler<GetUserRecentAcQuery, IReadOnlyList<UserRecentAcDto>>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserStatsReadService _userStatsReadService;

        public GetUserRecentAcQueryHandler(IUserReadRepository userReadRepository, IUserStatsReadService userStatsReadService)
        {
            _userReadRepository = userReadRepository;
            _userStatsReadService = userStatsReadService;
        }

        public async Task<IReadOnlyList<UserRecentAcDto>> Handle(GetUserRecentAcQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userReadRepository.GetUserIdByUsernameAsync(request.Username, cancellationToken);

            return await _userStatsReadService.GetUserRecentAcAsync(userId.Value, request.Count, cancellationToken);
        }
    }
}