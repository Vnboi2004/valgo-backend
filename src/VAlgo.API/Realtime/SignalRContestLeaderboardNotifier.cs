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
    }
}