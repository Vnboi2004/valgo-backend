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

        public GetContestLeaderboardQueryHandler(IContestRepository contestRepository, ILeaderboardService leaderboard, ILeaderboardCacheService cache)
        {
            _contestRepository = contestRepository;
            _leaderboard = leaderboard;
            _cache = cache;
        }

        public async Task<IReadOnlyList<ContestLeaderboardItemDto>> Handle(GetContestLeaderboardQuery request, CancellationToken cancellationToken)
        {
            var cached = await _cache.GetCachedTopAsync(request.ContestId);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<List<ContestLeaderboardItemDto>>(cached)!;
            }

            var data = await _leaderboard.GetTopAsync(request.ContestId, 100);

            var leaderboard = new List<ContestLeaderboardItemDto>();

            int rank = 1;

            foreach (var item in data)
            {
                leaderboard.Add(new ContestLeaderboardItemDto
                {
                    Rank = rank++,
                    UserId = item.UserId,
                    Score = item.Score,
                    Penalty = item.Penalty,
                    SolvedProblems = item.Score
                });
            }

            var json = JsonSerializer.Serialize(leaderboard);

            await _cache.CacheTopAsync(request.ContestId, json);

            return leaderboard;
        }
    }
}