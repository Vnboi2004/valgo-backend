using MediatR;
using VAlgo.Modules.Submissions.Application.DTOs;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemStats
{
    public sealed record GetProblemStatsQuery(Guid ProblemId) : IRequest<ProblemStatsDto>;
}