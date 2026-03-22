using MediatR;

namespace VAlgo.Modules.Identity.Application.Commands.UpdateUserExperience
{
    public sealed record UpdateUserExperienceCommand(
        string? Work,
        string? Education
    ) : IRequest<Unit>;
}