using MediatR;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard
{
    public sealed record GetContestLeaderboardQuery(Guid ContestId) : IRequest<IReadOnlyList<ContestLeaderboardItemDto>>;
}