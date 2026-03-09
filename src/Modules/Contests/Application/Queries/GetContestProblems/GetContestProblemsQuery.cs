using MediatR;
using VAlgo.Modules.Contests.Application.Queries.GetContestDetail;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestProblems
{
    public sealed record GetContestProblemsQuery(Guid ContestId) : IRequest<IReadOnlyList<ContestProblemDto>>;
}