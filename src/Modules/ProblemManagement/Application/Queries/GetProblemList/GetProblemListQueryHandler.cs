using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList
{
    public sealed class GetProblemListQueryHandler : IRequestHandler<GetProblemListQuery, PagedResult<ProblemListItemDto>>
    {
        private readonly IProblemReadStore _problemReadStore;

        public GetProblemListQueryHandler(IProblemReadStore problemReadStore)
            => _problemReadStore = problemReadStore;

        public async Task<PagedResult<ProblemListItemDto>> Handle(GetProblemListQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

            var skip = (page - 1) * pageSize;

            var (items, totalCount) = await _problemReadStore.GetProblemsAsync(request, skip, pageSize, cancellationToken);

            return new PagedResult<ProblemListItemDto>(items, page, pageSize, totalCount);
        }
    }
}