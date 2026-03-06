using MediatR;
using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.Modules.Contests.Application.Commands.UpdateContestVisibility
{
    public sealed record UpdateContestVisibilityCommand(
        Guid ContestId,
        ContestVisibility Visibility
    ) : IRequest<Unit>;
}