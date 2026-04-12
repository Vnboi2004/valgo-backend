using MediatR;
using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.Modules.Contests.Application.Commands.CreateContest
{
    public sealed record CreateContestCommand(
        string Title,
        string Description,
        string Code,
        DateTime StartTime,
        DateTime EndTime,
        ContestVisibility Visibility,
        ContestType Type
    ) : IRequest<Guid>;
}