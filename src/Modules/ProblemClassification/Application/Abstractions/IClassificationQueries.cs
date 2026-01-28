using VAlgo.Modules.ProblemClassification.Application.Queries.GetActiveClassifications;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetClassificationDetail;
using VAlgo.Modules.ProblemClassification.Application.Queries.GetClassifications;
using VAlgo.Modules.ProblemClassification.Domain.Enums;

namespace VAlgo.Modules.ProblemClassification.Application.Abstractions
{
    public interface IClassificationQueries
    {
        Task<(IReadOnlyList<ClassificationListItemDto>, int TotalCount)> GetListAsync(
            GetClassificationsQuery filter,
            int skip,
            int take,
            CancellationToken cancellationToken = default
        );
        Task<ClassificationDetailDto?> GetDetailAsync(Guid classificationId, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<ActiveClassificationDto>, int TotalCount)> GetActiveAsync(
            ClassificationType? type,
            int skip,
            int take,
            CancellationToken cancellationToken = default);
    }
}