using MediatR;

namespace VAlgo.Modules.Identity.Application.Queries.GetMyProfile
{
    public sealed record GetMyProfileQuery() : IRequest<MyProfileDto>;
}