using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.FinishContest
{
    public sealed record FinishContestCommand(Guid ContestId) : IRequest<Unit>;
}