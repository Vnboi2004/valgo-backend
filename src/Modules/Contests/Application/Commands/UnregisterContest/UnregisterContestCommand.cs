using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.UnregisterContest
{
    public sealed record UnregisterContestCommand(Guid ContestId) : IRequest<Unit>;
}