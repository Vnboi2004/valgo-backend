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

        private static double BuildScore(int score, int penalty)
        {
            return score * 1_000_000d - penalty;
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

                var composite = entry.Score;

                var score = (int)(composite / 1_000_000);

                var penatly = (int)(score * 1_000_000 - composite);

                result.Add(new LeaderboardEntry(userId, score, penatly));
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
    }
}