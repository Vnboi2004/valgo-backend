using MediatR;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.Modules.UserProfile.Application.Exceptions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserPracticeHistory
{
    public sealed class GetUserPracticeHistoryQueryHandler : IRequestHandler<GetUserPracticeHistoryQuery, UserPracticeHistoryDto>
    {
        private readonly IUserIdentityReadService _userIdentityReadService;
        private readonly IUserProfileReadService _userProfileReadService;

        public GetUserPracticeHistoryQueryHandler(IUserIdentityReadService userIdentityReadService, IUserProfileReadService userProfileReadService)
        {
            _userIdentityReadService = userIdentityReadService;
            _userProfileReadService = userProfileReadService;
        }

        public async Task<UserPracticeHistoryDto> Handle(GetUserPracticeHistoryQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userIdentityReadService.GetUserIdByUsernameAsync(request.Username, cancellationToken)
                ?? throw new UserProfileNotFoundException(request.Username);

            return await _userProfileReadService.GetUserPracticeHistoryAsync(
                userId,
                request.Page,
                request.PageSize,
                request.Status,
                request.Difficulty,
                cancellationToken
            );
        }
    }
}