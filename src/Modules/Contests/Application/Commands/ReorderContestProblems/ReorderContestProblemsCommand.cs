using MediatR;

namespace VAlgo.Modules.Contests.Application.Commands.ReorderContestProblems
{
    public sealed record ReorderContestProblemsCommand(
        Guid ContestId,
        IReadOnlyList<Guid> ProblemIds
    ) : IRequest<Unit>;
}