namespace VAlgo.Modules.Contests.Application.Realtime
{
    public interface IContestLeaderboardNotifier
    {
        Task NotifyLeaderboardUpdated(Guid contestId);
    }
}