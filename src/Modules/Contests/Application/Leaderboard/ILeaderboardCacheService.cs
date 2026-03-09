namespace VAlgo.Modules.Contests.Application.Leaderboard
{
    public interface ILeaderboardCacheService
    {
        Task CacheTopAsync(Guid contestId, string json);
        Task<string?> GetCachedTopAsync(Guid contestId);
    }
}