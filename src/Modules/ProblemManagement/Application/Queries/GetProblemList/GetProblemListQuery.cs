using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList
{
    public sealed record GetProblemListQuery(
        string? Keyword,
        Difficulty? Difficulty,
        ProblemStatus? Status,
        Guid? ClassificationId,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<PagedResult<ProblemListItemDto>>;
}