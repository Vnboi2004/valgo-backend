using MediatR;

namespace VAlgo.Modules.Identity.Application.Commands.UpdateAvatar
{
    public sealed record UpdateAvatarCommand(
        string AvatarUrl
    ) : IRequest<Unit>;
}