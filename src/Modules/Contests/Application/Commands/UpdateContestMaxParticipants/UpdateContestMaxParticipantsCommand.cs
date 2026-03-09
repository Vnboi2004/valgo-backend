using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.UpdateContestMaxParticipants
{
    public sealed record UpdateContestMaxParticipantsCommand(
        Guid ContestId,
        int? MaxParticipants
    ) : IRequest<Unit>;
}