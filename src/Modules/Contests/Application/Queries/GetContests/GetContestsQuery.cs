using MediatR;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Contests.Application.Queries.GetContests
{
    public sealed record GetContestsQuery(
        ContestPhase? Phase,
        ContestVisibility? Visibility,
        int Page,
        int PageSize
    ) : IRequest<PagedResult<ContestListItemDto>>;
}