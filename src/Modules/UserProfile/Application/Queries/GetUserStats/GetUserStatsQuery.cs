using MediatR;
using VAlgo.Modules.UserProfile.Application.DTOs;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserStats
{
    public sealed record GetUserStatsQuery(string Username) : IRequest<UserStatsDto>;
}