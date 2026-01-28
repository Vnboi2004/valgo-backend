using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionStatus
{
    public sealed class GetSubmissionStatusQueryHandler : IRequestHandler<GetSubmissionStatusQuery, SubmissionStatusDto>
    {
        private readonly ISubmissionQueries _submissionQueries;

        public GetSubmissionStatusQueryHandler(ISubmissionQueries submissionQueries)
            => _submissionQueries = submissionQueries;

        public async Task<SubmissionStatusDto> Handle(GetSubmissionStatusQuery request, CancellationToken cancellationToken)
        {
            var status = await _submissionQueries.GetStatusAsync(request.SubmissionId, cancellationToken);

            if (status == null)
                throw new InvalidOperationException($"Submission {request.SubmissionId} not found");

            return status;
        }
    }
}