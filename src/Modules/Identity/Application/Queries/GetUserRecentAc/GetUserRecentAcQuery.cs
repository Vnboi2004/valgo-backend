using MediatR;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserRecentAc
{
    public sealed record GetUserRecentAcQuery(string Username, int Count = 10) : IRequest<IReadOnlyList<UserRecentAcDto>>;
}