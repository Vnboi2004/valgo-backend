using VAlgo.Modules.ProblemClassification.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Application.Queries.GetActiveClassifications
{
    public sealed record GetActiveClassificationsQuery(
        ClassificationType? Type,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<PagedResult<ActiveClassificationDto>>;
}