using MediatR;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.Modules.UserProfile.Application.Exceptions;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserHeatmap
{
    public sealed class GetUserHeatmapQueryHandler : IRequestHandler<GetUserHeatmapQuery, IReadOnlyList<UserHeatmapItemDto>>
    {
        private readonly IUserIdentityReadService _userIdentityReadService;
        private readonly IUserProfileReadService _userProfileReadService;

        public GetUserHeatmapQueryHandler(IUserIdentityReadService userIdentityReadService, IUserProfileReadService userProfileReadService)
        {
            _userIdentityReadService = userIdentityReadService;
            _userProfileReadService = userProfileReadService;
        }

        public async Task<IReadOnlyList<UserHeatmapItemDto>> Handle(GetUserHeatmapQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userIdentityReadService.GetUserIdByUsernameAsync(request.Username, cancellationToken)
                ?? throw new UserProfileNotFoundException(request.Username);

            return await _userProfileReadService.GetUserHeatmapAsync(userId, cancellationToken);
        }
    }
}