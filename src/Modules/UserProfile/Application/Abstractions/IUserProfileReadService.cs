using VAlgo.Modules.UserProfile.Application.DTOs;
using VAlgo.Modules.UserProfile.Application.Queries.GetUserPracticeHistory;
using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.UserProfile.Application.Abstractions
{
    public interface IUserProfileReadService
    {
        Task<UserStatsDto> GetUserStatsAsync(Guid userId, CancellationToken cancellationToken);

        Task<IReadOnlyList<UserHeatmapItemDto>> GetUserHeatmapAsync(
            Guid userId,
            int year,
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

        Task<UserPracticeHistoryDto> GetUserPracticeHistoryAsync(
            Guid userId,
            int page,
            int pageSize,
            PracticeStatusFilter? status,
            Difficulty? difficulty,
            CancellationToken cancellationToken);
    }
}