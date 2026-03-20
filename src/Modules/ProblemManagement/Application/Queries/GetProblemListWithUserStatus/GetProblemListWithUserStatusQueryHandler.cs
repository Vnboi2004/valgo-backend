using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetUserProblemStatus;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemListWithUserStatus
{
    public sealed class GetProblemListWithUserStatusQueryHandler : IRequestHandler<GetProblemListWithUserStatusQuery, PagedResult<ProblemListStatusItemDto>>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;
        private readonly ISubmissionReadService _submissionReadService;
        private readonly ICurrentUserService _currentUserService;

        public GetProblemListWithUserStatusQueryHandler(
            IProblemManagementQueries problemManagementQueries,
            ISubmissionReadService submissionReadService,
            ICurrentUserService currentUserService
        )
        {
            _problemManagementQueries = problemManagementQueries;
            _submissionReadService = submissionReadService;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<ProblemListStatusItemDto>> Handle(GetProblemListWithUserStatusQuery request, CancellationToken cancellationToken)
        {
            var pagedProblems = await _problemManagementQueries.GetProblemListAsync(
                request.Keyword,
                request.Difficulty,
                request.Status,
                request.CompanyId,
                request.ClassificationId,
                request.SortBy,
                request.SortDirection,
                request.Page,
                request.PageSize,
                cancellationToken
            );

            Dictionary<Guid, UserProblemStatus> userStatusMap = new();

            if (_currentUserService.IsAuthenticated && pagedProblems.Items.Any())
            {
                var problemIds = pagedProblems.Items.Select(p => p.ProblemId).ToList();

                userStatusMap = await _submissionReadService.GetUserProblemStatusBatchAsync(
                    _currentUserService.UserId,
                    problemIds,
                    cancellationToken);
            }

            var enrichedItems = pagedProblems.Items.Select(p => new ProblemListStatusItemDto
            {
                ProblemId = p.ProblemId,
                Code = p.Code,
                Title = p.Title,
                ShortDescription = p.ShortDescription,
                Difficulty = p.Difficulty,
                Status = p.Status,
                TotalSubmissions = p.TotalSubmissions,
                AcceptedSubmissions = p.AcceptedSubmissions,
                UserStatus = _currentUserService.IsAuthenticated
                    ? userStatusMap.TryGetValue(p.ProblemId, out var status)
                        ? status
                        : UserProblemStatus.NotAttempted
                    : null
            }).ToList();

            return new PagedResult<ProblemListStatusItemDto>(
                enrichedItems,
                pagedProblems.TotalCount,
                request.Page,
                request.PageSize);
        }
    }
}