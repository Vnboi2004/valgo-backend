using System.Text.Json;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard;
using VAlgo.SharedKernel.Infrastructure.Redis;

namespace VAlgo.Modules.Contests.Infrastructure.Leaderboard
{
    public sealed class RedisLeaderboardSnapshotService : ILeaderboardSnapshotService
    {
        private readonly RedisDatabaseProvider _redis;

        public RedisLeaderboardSnapshotService(RedisDatabaseProvider redis)
        {
            _redis = redis;
        }

        private static string Key(Guid contestId)
            => $"contest:{contestId}:leaderboard:frozen";

        public async Task SaveSnapshotAsync(Guid contestId, IReadOnlyList<ContestLeaderboardItemDto> data)
        {
            var db = _redis.GetDatabase();

            var json = JsonSerializer.Serialize(data);

            await db.StringSetAsync(Key(contestId), json);
        }

        public async Task<IReadOnlyList<ContestLeaderboardItemDto>?> GetSnapshotAsync(Guid contestId)
        {
            var db = _redis.GetDatabase();

            var value = await db.StringGetAsync(Key(contestId));

            if (!value.HasValue)
                return null;

            return JsonSerializer.Deserialize<List<ContestLeaderboardItemDto>>(value!)!;
        }
    }
}