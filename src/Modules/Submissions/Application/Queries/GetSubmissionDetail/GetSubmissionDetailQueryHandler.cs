using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail
{
    public sealed class GetSubmissionDetailQueryHandler : IRequestHandler<GetSubmissionDetailQuery, SubmissionDetailDto>
    {
        private readonly ISubmissionQueries _submissionQueries;
        public GetSubmissionDetailQueryHandler(ISubmissionQueries submissionQueries)
            => _submissionQueries = submissionQueries;
        public async Task<SubmissionDetailDto> Handle(GetSubmissionDetailQuery request, CancellationToken cancellationToken)
        {
            var submission = await _submissionQueries.GetDetailAsync(request.SubmissionId, cancellationToken);

            if (submission == null)
                throw new InvalidOperationException($"Submission {request.SubmissionId} not found");

            if (submission.Status == SubmissionStatus.Completed)
            {
                var testCases = await _submissionQueries.GetTestCasesAsync(request.SubmissionId, cancellationToken);

                submission = submission with
                {
                    TestCases = testCases
                };
            }

            return submission;
        }
    }
}