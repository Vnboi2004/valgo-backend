using VAlgo.Modules.Identity.Application.Queries.GetUserHeatmap;
using VAlgo.Modules.Identity.Application.Queries.GetUserLanguages;
using VAlgo.Modules.Identity.Application.Queries.GetUserPracticeHistory;
using VAlgo.Modules.Identity.Application.Queries.GetUserRecentAc;
using VAlgo.Modules.Identity.Application.Queries.GetUserSkills;
using VAlgo.Modules.Identity.Application.Queries.GetUserStats;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Abstractions
{
    public interface IUserStatsReadService
    {
        Task<UserStatsDto> GetUserStatsAsync(Guid userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<UserHeatmapItemDto>> GetUserHeatmapAsync(Guid userId, CancellationToken cancellationToken);
        Task<IReadOnlyList<UserRecentAcDto>> GetUserRecentAcAsync(Guid userId, int count, CancellationToken cancellationToken);
        Task<IReadOnlyList<UserLanguageStatsDto>> GetUserLanguagesAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserSkillsDto> GetUserSkillsAsync(Guid userId, CancellationToken cancellationToken);
        Task<PagedResult<UserPracticeHistoryItemDto>> GetUserPracticeHistoryAsync(Guid userId, int page, int pageSize, CancellationToken cancellationToken);

    }
}