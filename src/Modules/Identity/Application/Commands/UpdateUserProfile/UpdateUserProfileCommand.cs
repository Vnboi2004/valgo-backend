using MediatR;
using VAlgo.Modules.Identity.Domain.Enums;

namespace VAlgo.Modules.Identity.Application.Commands.UpdateUserProfile
{
    public sealed record UpdateUserProfileCommand(
        string? DisplayName,
        Gender? Gender,
        string? Location,
        DateOnly? Birthday,
        string? Website,
        string? Github,
        string? LinkedIn,
        string? Twitter,
        string? ReadMe
    ) : IRequest<Unit>;
}