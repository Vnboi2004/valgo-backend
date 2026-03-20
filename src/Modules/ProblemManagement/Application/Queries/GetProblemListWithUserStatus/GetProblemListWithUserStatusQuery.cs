using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemListWithUserStatus
{
    public sealed record GetProblemListWithUserStatusQuery(
        string? Keyword,
        Difficulty? Difficulty,
        ProblemStatus? Status,
        Guid? CompanyId,
        Guid? ClassificationId,
        ProblemSortBy SortBy,
        SortDirection SortDirection,
        int Page,
        int PageSize
    ) : IRequest<PagedResult<ProblemListStatusItemDto>>;
}