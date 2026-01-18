using MediatR;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.Enums;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail
{
    public sealed class GetSubmissionDetailQueryHandler : IRequestHandler<GetSubmissionDetailQuery, SubmissionDetailDto>
    {
        private readonly ISubmissionDetailReadStore _submissionDetailReadStore;
        private readonly IJudgeResultReadStore _judgeResultReadStore;

        public GetSubmissionDetailQueryHandler(
            ISubmissionDetailReadStore submissionDetailReadStore,
            IJudgeResultReadStore judgeResultReadStore
        )
        {
            _submissionDetailReadStore = submissionDetailReadStore;
            _judgeResultReadStore = judgeResultReadStore;
        }

        public async Task<SubmissionDetailDto> Handle(GetSubmissionDetailQuery request, CancellationToken cancellationToken)
        {
            var submission = await _submissionDetailReadStore.GetByIdAsync(request.SubmissionId, cancellationToken);

            if (submission == null)
                throw new InvalidOperationException($"Submission {request.SubmissionId} not found");

            if (submission.Status == SubmissionStatus.Completed)
            {
                var testCases = await _judgeResultReadStore.GetTestCasesAsync(request.SubmissionId, cancellationToken);

                submission = submission with
                {
                    TestCases = testCases
                };
            }

            return submission;
        }
    }
}