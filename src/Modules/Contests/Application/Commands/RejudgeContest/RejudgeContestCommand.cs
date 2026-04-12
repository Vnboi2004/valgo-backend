using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.RejudgeContest
{
    public sealed record RejudgeContestCommand(Guid ContestId) : IRequest<Unit>;
}