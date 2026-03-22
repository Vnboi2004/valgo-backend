using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserLanguages
{
    public sealed class GetUserLanguagesQueryHandle : IRequestHandler<GetUserLanguagesQuery, IReadOnlyList<UserLanguageStatsDto>>
    {
        private readonly IUserReadRepository _userReadRepository;
        private readonly IUserStatsReadService _userStatsReadService;

        public GetUserLanguagesQueryHandle(IUserReadRepository userReadRepository, IUserStatsReadService userStatsReadService)
        {
            _userReadRepository = userReadRepository;
            _userStatsReadService = userStatsReadService;
        }

        public async Task<IReadOnlyList<UserLanguageStatsDto>> Handle(GetUserLanguagesQuery request, CancellationToken cancellationToken)
        {
            var userId = await _userReadRepository.GetUserIdByUsernameAsync(request.Username, cancellationToken);

            if (userId == null)
                throw new InvalidOperationException("User not found.");

            return await _userStatsReadService.GetUserLanguagesAsync(userId.Value, cancellationToken);
        }
    }
}