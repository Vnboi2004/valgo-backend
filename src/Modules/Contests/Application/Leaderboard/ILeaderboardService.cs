namespace VAlgo.Modules.Contests.Application.Leaderboard
{
    public interface ILeaderboardService
    {
        Task UpdateParticipantAsync(Guid contestId, Guid userId, int score, int penalty, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<LeaderboardEntry>> GetTopAsync(Guid contestId, int top, CancellationToken cancellationToken = default);
        Task<long?> GetRankAsync(Guid contestId, Guid userId, CancellationToken cancellationToken = default);
    }
}