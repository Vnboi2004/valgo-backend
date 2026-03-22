using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserStats
{
    public sealed class GetUserStatsQueryHandler : IRequestHandler<GetUserStatsQuery, UserStatsDto>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserStatsReadService _userStatsReadService;

        public GetUserStatsQueryHandler(IUserReadRepository userReadRepository, IUserStatsReadService userStatsReadService)
        {
            _userReadRepository = userReadRepository;
            _userStatsReadService = userStatsReadService;
        }

        public async Task<UserStatsDto> Handle(GetUserStatsQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userReadRepository.GetUserIdByUsernameAsync(request.UserName, cancellationToken);

            if (userId == null)
                throw new InvalidOperationException("User not found.");

            return await _userStatsReadService.GetUserStatsAsync(userId.Value, cancellationToken);
        }
    }
}