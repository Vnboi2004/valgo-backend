using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetUserProblemStatus;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditorial
{
    public sealed record GetProblemEditorialQueryHandler : IRequestHandler<GetProblemEditorialQuery, ProblemEditorialDto>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;
        private readonly ISubmissionReadService _submissionReadService;
        private readonly ICurrentUserService _currentUserService;

        public GetProblemEditorialQueryHandler(
            IProblemManagementQueries problemManagementQueries,
            ISubmissionReadService submissionReadService,
            ICurrentUserService currentUserService
        )
        {
            _problemManagementQueries = problemManagementQueries;
            _submissionReadService = submissionReadService;
            _currentUserService = currentUserService;
        }

        public async Task<ProblemEditorialDto> Handle(GetProblemEditorialQuery request, CancellationToken cancellationToken)
        {
            var editorial = await _problemManagementQueries.GetEditorialAsync(request.ProblemId, cancellationToken)
                ?? throw new InvalidOperationException($"Problem not found {request.ProblemId}");

            if (string.IsNullOrWhiteSpace(editorial.Editorial))
                throw new InvalidOperationException($"Editorial not found {request.ProblemId}");

            if (!_currentUserService.IsInRole("Admin") || _currentUserService.IsInRole("ProblemSetter"))
                return editorial;

            if (!_currentUserService.IsAuthenticated)
                throw new InvalidOperationException($"Editorial access denied {request.ProblemId}");

            var userStatus = await _submissionReadService.GetUserProblemStatusAsync(_currentUserService.UserId, request.ProblemId, cancellationToken);

            if (userStatus != UserProblemStatus.Solved)
                throw new InvalidOperationException($"Editorial access denied {request.ProblemId}");

            return editorial;
        }
    }
}