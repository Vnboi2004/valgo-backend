using System.ComponentModel;
using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.DTOs;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemStats
{
    public sealed class GetProblemStatsQueryHandler : IRequestHandler<GetProblemStatsQuery, ProblemStatsDto>
    {
        private readonly ISubmissionQueries _submissionQueries;

        public GetProblemStatsQueryHandler(ISubmissionQueries submissionQueries)
            => _submissionQueries = submissionQueries;

        public async Task<ProblemStatsDto> Handle(GetProblemStatsQuery request, CancellationToken cancellationToken)
        {
            return await _submissionQueries.GetProblemStatsAsync(request.ProblemId, cancellationToken);
        }
    }
}