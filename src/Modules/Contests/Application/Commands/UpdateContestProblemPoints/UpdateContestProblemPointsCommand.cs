using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.UpdateContestProblemPoints
{
    public sealed record UpdateContestProblemPointsCommand(
        Guid ContestId,
        Guid ProblemId,
        int Points
    ) : IRequest<Unit>;
}