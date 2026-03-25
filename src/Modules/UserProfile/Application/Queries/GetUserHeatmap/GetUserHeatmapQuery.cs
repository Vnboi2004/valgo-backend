using MediatR;
using VAlgo.Modules.UserProfile.Application.DTOs;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserHeatmap
{
    public sealed record GetUserHeatmapQuery(string Username, int? Year) : IRequest<UserHeatmapDto>;
}