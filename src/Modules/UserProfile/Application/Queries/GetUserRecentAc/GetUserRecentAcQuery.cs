using MediatR;
using VAlgo.Modules.UserProfile.Application.DTOs;

namespace VAlgo.Modules.UserProfile.Application.Queries.GetUserRecentAc
{
    public sealed record GetUserRecentAcQuery(string Username, int Count = 10) : IRequest<IReadOnlyList<UserRecentAcDto>>;
}