using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.FreezeLeaderboard
{
    public sealed record FreezeLeaderboardCommand(Guid ContestId) : IRequest<Unit>;
}