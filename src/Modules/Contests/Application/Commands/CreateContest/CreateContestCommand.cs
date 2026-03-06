using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.CreateContest
{
    public sealed record CreateContestCommand(
        string Title,
        string Description,
        DateTime StartTime,
        DateTime EndTime,
        Guid CreatedBy
    ) : IRequest<Guid>;
}