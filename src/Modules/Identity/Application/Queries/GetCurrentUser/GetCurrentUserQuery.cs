using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Application.Queries.GetCurrentUser
{
    public sealed record GetCurrentUserQuery(Guid UserId) : IQuery<CurrentUserResponse>;
}