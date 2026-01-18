using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissions
{
    public sealed class GetSubmissionsQueryHandler : IRequestHandler<GetSubmissionsQuery, PagedResult<SubmissionListItemDto>>
    {
        private readonly ISubmissionReadStore _submissionReadStore;

        public GetSubmissionsQueryHandler(ISubmissionReadStore submissionReadStore)
            => _submissionReadStore = submissionReadStore;

        public async Task<PagedResult<SubmissionListItemDto>> Handle(GetSubmissionsQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;

            var skip = (page - 1) * pageSize;

            var (items, totalCount) = await _submissionReadStore.GetSubmissionsAsync(request.UserId, request.ProblemId, skip, pageSize, cancellationToken);

            return new PagedResult<SubmissionListItemDto>(items, page, pageSize, totalCount);
        }
    }
}