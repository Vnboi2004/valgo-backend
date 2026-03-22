using MediatR;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserHeatmap
{
    public sealed record GetUserHeatmapQuery(string Username) : IRequest<IReadOnlyList<UserHeatmapItemDto>>;
}