using Microsoft.AspNetCore.Mvc;
using VAlgo.Modules.Contests.Application.Leaderboard;

namespace VAlgo.API.Controllers
{
    [ApiController]
    [Route("api/leaderboard-test")]
    public sealed class LeaderboardTestController : Controller
    {
        private readonly ILeaderboardService _leaderboard;

        public LeaderboardTestController(ILeaderboardService leaderboard)
        {
            _leaderboard = leaderboard;
        }

        [HttpPost("score")]
        public async Task<IActionResult> UpdateScore(Guid contestId, Guid userId, int score, int penalty)
        {
            await _leaderboard.UpdateParticipantAsync(contestId, userId, score, penalty);

            return Ok();
        }

        [HttpGet("top")]
        public async Task<IActionResult> Top(Guid contestId)
        {
            var data = await _leaderboard.GetTopAsync(contestId, 10);

            return Ok(data);
        }

        [HttpGet("rank")]
        public async Task<IActionResult> Rank(Guid contestId, Guid userId)
        {
            var rank = await _leaderboard.GetRankAsync(contestId, userId);

            return Ok(rank);
        }
    }
}