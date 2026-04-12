using System.Text.Json;
using MediatR;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard
{
    public sealed class GetContestLeaderboardQueryHandler : IRequestHandler<GetContestLeaderboardQuery, IReadOnlyList<ContestLeaderboardItemDto>>
    {
        private readonly IContestRepository _contestRepository;
        private readonly ILeaderboardService _leaderboard;
        private readonly ILeaderboardCacheService _cache;
        private readonly ILeaderboardSnapshotService _snapshot;

        public GetContestLeaderboardQueryHandler(IContestRepository contestRepository, ILeaderboardService leaderboard, ILeaderboardCacheService cache, ILeaderboardSnapshotService snapshot)
        {
            _contestRepository = contestRepository;
            _leaderboard = leaderboard;
            _cache = cache;
            _snapshot = snapshot;
        }

        public async Task<IReadOnlyList<ContestLeaderboardItemDto>> Handle(
            GetContestLeaderboardQuery request,
            CancellationToken cancellationToken)
        {
            // 1. Get contest
            var contest = await _contestRepository.GetByIdAsync(ContestId.From(request.ContestId), cancellationToken);

            if (contest == null)
                throw new InvalidOperationException("Contest not found.");

            // 2. Nếu freeze → trả snapshot
            if (contest.IsLeaderboardFrozen)
            {
                var snapshot = await _snapshot.GetSnapshotAsync(request.ContestId);

                if (snapshot != null)
                    return snapshot;
            }

            // 3. Cache
            var cached = await _cache.GetCachedTopAsync(request.ContestId);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<List<ContestLeaderboardItemDto>>(cached)!;
            }

            // 4. Get live leaderboard
            var data = await _leaderboard.GetTopAsync(request.ContestId, 100);

            var userIds = data.Select(x => x.UserId).ToList();

            var solvedMap = await _leaderboard.GetSolvedMapAsync(
                request.ContestId,
                userIds);

            var result = new List<ContestLeaderboardItemDto>();

            int currentRank = 0;
            int index = 0;

            int? lastScore = null;
            int? lastPenalty = null;

            foreach (var item in data)
            {
                index++;

                // Tie ranking (ICPC style)
                if (lastScore != item.Score || lastPenalty != item.Penalty)
                {
                    currentRank = index;
                    lastScore = item.Score;
                    lastPenalty = item.Penalty;
                }

                result.Add(new ContestLeaderboardItemDto
                {
                    Rank = currentRank,
                    UserId = item.UserId,
                    Score = item.Score,
                    Penalty = item.Penalty,
                    SolvedProblems = solvedMap.GetValueOrDefault(item.UserId, 0)
                });
            }

            // 5. Cache lại
            var json = JsonSerializer.Serialize(result);

            await _cache.CacheTopAsync(request.ContestId, json);

            return result;
        }
    }
}