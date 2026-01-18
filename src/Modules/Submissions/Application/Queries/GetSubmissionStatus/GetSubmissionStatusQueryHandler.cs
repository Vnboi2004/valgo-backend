using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionStatus
{
    public sealed class GetSubmissionStatusQueryHandler : IRequestHandler<GetSubmissionStatusQuery, SubmissionStatusDto>
    {
        private readonly ISubmissionStatusReadStore _submissionStatusReadStore;

        public GetSubmissionStatusQueryHandler(ISubmissionStatusReadStore submissionStatusReadStore)
            => _submissionStatusReadStore = submissionStatusReadStore;

        public async Task<SubmissionStatusDto> Handle(GetSubmissionStatusQuery request, CancellationToken cancellationToken)
        {
            var status = await _submissionStatusReadStore.GetStatusAsync(request.SubmissionId, cancellationToken);

            if (status == null)
                throw new InvalidOperationException($"Submission {request.SubmissionId} not found");

            return status;
        }
    }
}