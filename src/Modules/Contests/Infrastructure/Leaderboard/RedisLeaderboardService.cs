using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata;
using StackExchange.Redis;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.SharedKernel.Infrastructure.Redis;

namespace VAlgo.Modules.Contests.Infrastructure.Leaderboard
{
    public sealed class RedisLeaderboardService : ILeaderboardService
    {
        private readonly RedisDatabaseProvider _redis;

        public RedisLeaderboardService(RedisDatabaseProvider redis)
        {
            _redis = redis;
        }

        private static string GetKey(Guid contestId)
            => $"contest:{contestId}:leaderboard";

        private static string SolvedKey(Guid contestId)
            => $"contest:{contestId}:solved";

        private static long BuildScore(int score, int penalty)
        {
            return ((long)score << 32) - penalty;
        }

        public async Task UpdateParticipantAsync(Guid contestId, Guid userId, int score, int penalty, CancellationToken cancellationToken)
        {
            var db = _redis.GetDatabase();

            var composite = BuildScore(score, penalty);

            await db.SortedSetAddAsync(GetKey(contestId), userId.ToString(), composite);
        }

        public async Task<IReadOnlyList<LeaderboardEntry>> GetTopAsync(Guid contestId, int top, CancellationToken cancellationToken)
        {
            var db = _redis.GetDatabase();

            var entries = await db.SortedSetRangeByRankWithScoresAsync(GetKey(contestId), 0, top - 1, Order.Descending);

            var result = new List<LeaderboardEntry>();

            foreach (var entry in entries)
            {
                var userId = Guid.Parse(entry.Element!);

                var composite = (long)entry.Score;

                var score = (int)(composite >> 32);

                var penatly = (int)(composite & 0xffffffff);

                result.Add(new LeaderboardEntry(userId, score, penatly, 0)); // solvedProblems lấy từ db
            }

            return result;
        }

        public async Task<long?> GetRankAsync(Guid contestId, Guid userId, CancellationToken cancellationToken)
        {
            var db = _redis.GetDatabase();

            var rank = await db.SortedSetRankAsync(GetKey(contestId), userId.ToString(), Order.Descending);

            if (rank == null)
                return null;

            return rank.Value + 1;
        }

        public async Task<Dictionary<Guid, int>> GetSolvedMapAsync(Guid contestId, List<Guid> userIds, CancellationToken cancellationToken = default)
        {
            var db = _redis.GetDatabase();

            var key = SolvedKey(contestId);

            var fields = userIds.Select(x => (RedisValue)x.ToString()).ToArray();

            var values = await db.HashGetAsync(key, fields);

            var result = new Dictionary<Guid, int>();

            for (int i = 0; i < userIds.Count; i++)
            {
                result[userIds[i]] = values[i].HasValue ? (int)values[i] : 0;
            }

            return result;
        }
    }
}