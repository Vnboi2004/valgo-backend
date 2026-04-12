using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.RemoveParticipant
{
    public sealed record RemoveParticipantCommand(Guid ContestId, Guid UserId) : IRequest<Unit>;
}