using MediatR;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.Modules.UserProfile.Application.Exceptions;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserLanguages
{
    public sealed class GetUserLanguagesQueryHandler : IRequestHandler<GetUserLanguagesQuery, IReadOnlyList<UserLanguageStatsDto>>
    {
        private readonly IUserIdentityReadService _userIdentityReadService;
        private readonly IUserProfileReadService _userProfileReadService;

        public GetUserLanguagesQueryHandler(IUserIdentityReadService userIdentityReadService, IUserProfileReadService userProfileReadService)
        {
            _userIdentityReadService = userIdentityReadService;
            _userProfileReadService = userProfileReadService;
        }

        public async Task<IReadOnlyList<UserLanguageStatsDto>> Handle(GetUserLanguagesQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userIdentityReadService.GetUserIdByUsernameAsync(request.Username, cancellationToken)
                ?? throw new UserProfileNotFoundException(request.Username);

            return await _userProfileReadService.GetUserLanguagesAsync(userId, cancellationToken);
        }
    }
}