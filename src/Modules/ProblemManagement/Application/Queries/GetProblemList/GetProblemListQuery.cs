using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList
{
    public sealed record GetProblemListQuery(
        string? Keyword,
        Difficulty? Difficulty,
        ProblemStatus? Status,
        Guid? CompanyId,
        Guid? ClassificationId,
        ProblemSortBy SortBy = ProblemSortBy.Code,
        SortDirection SortDirection = SortDirection.Asc,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<PagedResult<ProblemListItemDto>>;
}