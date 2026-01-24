using MediatR;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemClassification.Application.Queries.GetActiveClassifications
{
    public sealed class GetActiveClassificationsQueryHandler : IRequestHandler<GetActiveClassificationsQuery, PagedResult<ActiveClassificationDto>>
    {
        private readonly IClassificationQueries _classificationQueries;

        public GetActiveClassificationsQueryHandler(IClassificationQueries classificationQueries)
            => _classificationQueries = classificationQueries;

        public async Task<PagedResult<ActiveClassificationDto>> Handle(GetActiveClassificationsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 0 : request.Page;
            var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

            var skip = (page - 1) * pageSize;

            var (items, totalCount) = await _classificationQueries.GetActiveAsync(request.Type, skip, pageSize, cancellationToken);

            return new PagedResult<ActiveClassificationDto>(items, page, pageSize, totalCount);
        }
    }
}