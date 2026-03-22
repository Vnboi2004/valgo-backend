using MediatR;

namespace VAlgo.Modules.Identity.Application.Commands.UpdateAvatar
{
    public sealed record UpdateAvatarCommand(
        Guid UserId,
        string AvatarUrl
    ) : IRequest<Unit>;
}