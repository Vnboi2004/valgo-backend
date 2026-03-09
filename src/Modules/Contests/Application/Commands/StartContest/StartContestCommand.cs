using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.StartContest
{
    public sealed record StartContestCommand(Guid ContestId) : IRequest<Unit>;
}