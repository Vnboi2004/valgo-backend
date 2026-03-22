using MediatR;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.Modules.UserProfile.Application.Exceptions;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserSkills
{
    public sealed class GetUserSkillsQueryHandler : IRequestHandler<GetUserSkillsQuery, UserSkillsDto>
    {
        private readonly IUserIdentityReadService _userIdentityReadService;
        private readonly IUserProfileReadService _userProfileReadService;

        public GetUserSkillsQueryHandler(IUserIdentityReadService userIdentityReadService, IUserProfileReadService userProfileReadService)
        {
            _userIdentityReadService = userIdentityReadService;
            _userProfileReadService = userProfileReadService;
        }

        public async Task<UserSkillsDto> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userIdentityReadService.GetUserIdByUsernameAsync(request.Username, cancellationToken)
                ?? throw new UserProfileNotFoundException(request.Username);

            return await _userProfileReadService.GetUserSkillsAsync(userId, cancellationToken);
        }
    }
}