using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.SharedKernel.Infrastructure.Redis;

namespace VAlgo.Modules.Contests.Infrastructure.Leaderboard
{
    public sealed class RedisLeaderboardCacheService : ILeaderboardCacheService
    {
        private readonly RedisDatabaseProvider _redis;

        public RedisLeaderboardCacheService(RedisDatabaseProvider redis)
        {
            _redis = redis;
        }

        private static string Key(Guid contestId)
            => $"contest:{contestId}:leaderboard:top";

        public async Task CacheTopAsync(Guid contestId, string json)
        {
            var db = _redis.GetDatabase();

            await db.StringSetAsync(Key(contestId), json, TimeSpan.FromSeconds(10));
        }

        public async Task<string?> GetCachedTopAsync(Guid contestId)
        {
            var db = _redis.GetDatabase();

            var value = await db.StringGetAsync(Key(contestId));

            return value.HasValue ? value.ToString() : null;
        }

    }
}