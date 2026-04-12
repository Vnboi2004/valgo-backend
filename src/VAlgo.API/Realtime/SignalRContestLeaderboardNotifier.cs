using Microsoft.AspNetCore.SignalR;
using VAlgo.API.Hubs;
using VAlgo.Modules.Contests.Application.Realtime;

namespace VAlgo.API.Realtime
{
    public sealed class SignalRContestLeaderboardNotifier : IContestLeaderboardNotifier
    {
        private readonly IHubContext<ContestLeaderboardHub> _hub;

        public SignalRContestLeaderboardNotifier(IHubContext<ContestLeaderboardHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyLeaderboardUpdated(Guid contestId)
        {
            await _hub.Clients
                .Group($"contest-{contestId}")
                .SendAsync("LeaderboardUpdated", contestId);
        }

        public async Task NotifyContestStarted(Guid contestId)
        {
            await _hub.Clients
                .Group($"contest-{contestId}")
                .SendAsync("contest_started", contestId);
        }

        public async Task NotifyContestFinished(Guid contestId)
        {
            await _hub.Clients
                .Group($"contest-{contestId}")
                .SendAsync("contest_finished", contestId);
        }

        public async Task NotifyLeaderboardFrozen(Guid contestId)
        {
            await _hub.Clients
                .Group($"contest-{contestId}")
                .SendAsync("leaderboard_frozen", contestId);
        }
    }
}