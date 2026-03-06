using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.JoinContest
{
    public sealed record JoinContestCommand(
        Guid ContestId,
        Guid UserId
    ) : IRequest<Unit>;
}