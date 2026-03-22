using MediatR;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.Modules.UserProfile.Application.Exceptions;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserRecentAc
{
    public sealed class GetUserRecentAcQueryHandler : IRequestHandler<GetUserRecentAcQuery, IReadOnlyList<UserRecentAcDto>>
    {
        private readonly IUserIdentityReadService _userIdentityReadService;
        private readonly IUserProfileReadService _userProfileReadService;

        public GetUserRecentAcQueryHandler(IUserIdentityReadService userIdentityReadService, IUserProfileReadService userProfileReadService)
        {
            _userIdentityReadService = userIdentityReadService;
            _userProfileReadService = userProfileReadService;
        }

        public async Task<IReadOnlyList<UserRecentAcDto>> Handle(GetUserRecentAcQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userIdentityReadService.GetUserIdByUsernameAsync(request.Username, cancellationToken)
                ?? throw new UserProfileNotFoundException(request.Username);

            return await _userProfileReadService.GetUserRecentAcAsync(userId, request.Count, cancellationToken);
        }
    }
}