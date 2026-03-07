namespace VAlgo.Modules.Contests.Application.Leaderboard
{
    public interface ILeaderboardService
    {
        Task UpdateParticipantAsync(Guid contestId, Guid userId, int score, int penalty, CancellationToken cancellationToken);
        Task<IReadOnlyList<LeaderboardEntry>> GetTopAsync(Guid contestId, int limit, CancellationToken cancellationToken);
        Task<long> GetRankAsync(Guid contestId, Guid userId, CancellationToken cancellationToken);
    }
}