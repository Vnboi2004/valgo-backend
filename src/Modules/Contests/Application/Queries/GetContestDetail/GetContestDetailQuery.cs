using MediatR;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestDetail
{
    public sealed record GetContestDetailQuery(Guid ContestId) : IRequest<ContestDetailDto>;
}