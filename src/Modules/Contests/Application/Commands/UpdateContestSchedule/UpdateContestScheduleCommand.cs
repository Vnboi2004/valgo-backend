using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.UpdateContestSchedule
{
    public sealed record UpdateContestScheduleCommand(
        Guid ContestId,
        DateTime StartTime,
        DateTime EndTime
    ) : IRequest<Unit>;
}