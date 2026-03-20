using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetUserProblemStatus
{
    public sealed class GetUserProblemStatusQueryHandler : IRequestHandler<GetUserProblemStatusQuery, UserProblemStatusDto>
    {
        private readonly ISubmissionReadService _submissionReadService;
        private readonly ICurrentUserService _currentUserService;

        public GetUserProblemStatusQueryHandler(ISubmissionReadService submissionReadService, ICurrentUserService currentUserService)
        {
            _submissionReadService = submissionReadService;
            _currentUserService = currentUserService;
        }

        public async Task<UserProblemStatusDto> Handle(GetUserProblemStatusQuery request, CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated)
            {
                return new UserProblemStatusDto
                {
                    ProblemId = request.ProblemId,
                    Status = null
                };
            }

            var status = await _submissionReadService.GetUserProblemStatusAsync(_currentUserService.UserId, request.ProblemId, cancellationToken);

            return new UserProblemStatusDto
            {
                ProblemId = request.ProblemId,
                Status = status
            };
        }
    }
}