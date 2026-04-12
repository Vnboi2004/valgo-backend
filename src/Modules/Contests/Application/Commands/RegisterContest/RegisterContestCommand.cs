using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.RegisterContest
{
    public sealed record RegisterContestCommand(Guid ContestId) : IRequest<Unit>;
}