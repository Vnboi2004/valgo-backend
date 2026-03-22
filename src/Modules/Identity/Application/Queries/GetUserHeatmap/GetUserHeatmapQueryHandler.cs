using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserHeatmap
{
    public sealed class GetUserHeatmapQueryHandler : IRequestHandler<GetUserHeatmapQuery, IReadOnlyList<UserHeatmapItemDto>>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserStatsReadService _userStatsReadService;

        public GetUserHeatmapQueryHandler(IUserReadRepository userReadRepository, IUserStatsReadService userStatsReadService)
        {
            _userReadRepository = userReadRepository;
            _userStatsReadService = userStatsReadService;
        }

        public async Task<IReadOnlyList<UserHeatmapItemDto>> Handle(GetUserHeatmapQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userReadRepository.GetUserIdByUsernameAsync(request.Username, cancellationToken);

            if (userId == null)
                throw new InvalidOperationException("User not found.");

            return await _userStatsReadService.GetUserHeatmapAsync(userId.Value, cancellationToken);
        }
    }
}