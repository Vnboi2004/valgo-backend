using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.RemoveProblemFromContest
{
    public sealed record RemoveProblemFromContestCommand(
        Guid ContestId,
        Guid ProblemId
    ) : IRequest<Unit>;
}