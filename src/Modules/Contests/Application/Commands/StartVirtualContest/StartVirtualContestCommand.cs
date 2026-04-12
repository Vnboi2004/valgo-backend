using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.StartVirtualContest
{
    public sealed record StartVirtualContestCommand(Guid ContestId) : IRequest<Guid>;
}