using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserSkills
{
    public sealed class GetUserSkillsQueryHandler : IRequestHandler<GetUserSkillsQuery, UserSkillsDto>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserStatsReadService _userStatsReadService;

        public GetUserSkillsQueryHandler(IUserReadRepository userReadRepository, IUserStatsReadService userStatsReadService)
        {
            _userReadRepository = userReadRepository;
            _userStatsReadService = userStatsReadService;
        }

        public async Task<UserSkillsDto> Handle(GetUserSkillsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userReadRepository.GetUserIdByUsernameAsync(request.Username, cancellationToken);

            if (userId == null)
                throw new InvalidOperationException("User not found.");

            return await _userStatsReadService.GetUserSkillsAsync(userId.Value, cancellationToken);
        }
    }
}