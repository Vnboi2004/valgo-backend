using VAlgo.Modules.Identity.Domain.Enums;

namespace VAlgo.API.Controllers.Identity.Requests
{
    public sealed record UpdateUserProfileRequest(
        string? DisplayName,
        Gender? Gender,
        string? Location,
        DateOnly? Birthday,
        string? Website,
        string? Github,
        string? LinkedIn,
        string? Twitter,
        string? ReadMe
    );
}