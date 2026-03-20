using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Classifications;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Application.Queries.GetClassifications
{
    public sealed record GetClassificationsQuery(
        ClassificationType? Type,
        bool? IsActive,
        int Page = 1,
        int PageSize = 20
    ) : IQuery<PagedResult<ClassificationListItemDto>>;
}