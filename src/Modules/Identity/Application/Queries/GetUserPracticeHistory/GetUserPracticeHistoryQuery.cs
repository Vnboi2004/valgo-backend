using MediatR;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserPracticeHistory
{
    public sealed record GetUserPracticeHistoryQuery(
        string Username,
        int Page = 1,
        int PageSize = 20
    ) : IRequest<PagedResult<UserPracticeHistoryItemDto>>;
}