using MediatR;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestMe
{
    public sealed record GetContestMeQuery(Guid ContestId) : IRequest<ContestMeDto>;
}