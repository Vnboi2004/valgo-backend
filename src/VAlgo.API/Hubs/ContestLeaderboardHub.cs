using Microsoft.AspNetCore.SignalR;

namespace VAlgo.API.Hubs
{
    public sealed class ContestLeaderboardHub : Hub
    {
        public async Task JoinContest(string contestId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"contest-{contestId}");
        }

        public async Task LeaveContest(string contestId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"contest-{contestId}");
        }
    }
}