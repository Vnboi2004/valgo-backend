namespace VAlgo.Modules.Contests.Application.Realtime
{
    public interface IContestLeaderboardNotifier
    {
        Task NotifyLeaderboardUpdated(Guid contestId);
        Task NotifyContestStarted(Guid contestId);
        Task NotifyContestFinished(Guid contestId);
        Task NotifyLeaderboardFrozen(Guid contestId);
    }
}