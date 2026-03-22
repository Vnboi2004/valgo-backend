using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.UserProfile.Application.Abstractions
{
    public interface IUserProfileReadService
    {
        Task<UserStatsDto> GetUserStatsAsync(Guid userId, CancellationToken cancellationToken);

        Task<IReadOnlyList<UserHeatmapItemDto>> GetUserHeatmapAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<UserRecentAcDto>> GetUserRecentAcAsync(
            Guid userId,
            int count,
            CancellationToken cancellationToken);

        Task<IReadOnlyList<UserLanguageStatsDto>> GetUserLanguagesAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<UserSkillsDto> GetUserSkillsAsync(
            Guid userId,
            CancellationToken cancellationToken);

        Task<PagedResult<UserPracticeHistoryItemDto>> GetUserPracticeHistoryAsync(
            Guid userId,
            int page,
            int pageSize,
            CancellationToken cancellationToken);
    }
}