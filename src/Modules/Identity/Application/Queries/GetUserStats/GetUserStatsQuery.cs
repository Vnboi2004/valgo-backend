using MediatR;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserStats
{
    public sealed record GetUserStatsQuery(string UserName) : IRequest<UserStatsDto>;
}