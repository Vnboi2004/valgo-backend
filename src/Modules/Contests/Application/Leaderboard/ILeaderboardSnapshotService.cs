using VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard;

namespace VAlgo.Modules.Contests.Application.Leaderboard
{
    public interface ILeaderboardSnapshotService
    {
        Task SaveSnapshotAsync(Guid contestId, IReadOnlyList<ContestLeaderboardItemDto> data);
        Task<IReadOnlyList<ContestLeaderboardItemDto>?> GetSnapshotAsync(Guid contestId);
    }
}