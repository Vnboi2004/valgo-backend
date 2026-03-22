using MediatR;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserPublicProfile
{
    public sealed record GetUserPublicProfileQuery(string UserName) : IRequest<UserPublicProfileDto>;
}