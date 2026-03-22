using MediatR;

namespace VAlgo.Modules.Identity.Application.Commands.UpdateUserExperience
{
    public sealed record UpdateUserExperienceCommand(
        Guid UserId,
        string? Work,
        string? Education
    ) : IRequest<Unit>;
}