using MediatR;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Application.Queries.GetClassifications
{
    public sealed class GetClassificationsQueryHandler : IRequestHandler<GetClassificationsQuery, PagedResult<ClassificationListItemDto>>
    {
        private readonly IClassificationQueries _classificationQueries;

        public GetClassificationsQueryHandler(IClassificationQueries classificationQueries)
            => _classificationQueries = classificationQueries;

        public async Task<PagedResult<ClassificationListItemDto>> Handle(GetClassificationsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

            var skip = (page - 1) * pageSize;

            var (items, totalCount) = await _classificationQueries.GetListAsync(request, skip, pageSize, cancellationToken);

            return new PagedResult<ClassificationListItemDto>(items, page, pageSize, totalCount);
        }
    }
}