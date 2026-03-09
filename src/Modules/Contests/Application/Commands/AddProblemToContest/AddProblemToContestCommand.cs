using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.AddProblemToContest
{
    public sealed record AddProblemToContestCommand(
        Guid ContestId,
        Guid ProblemId,
        string Code,
        int Points
    ) : IRequest<Guid>;
}