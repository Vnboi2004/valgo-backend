using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.LeaveContest
{
    public sealed record LeaveContestCommand(
        Guid ContestId,
        Guid UserId
    ) : IRequest<Unit>;
}