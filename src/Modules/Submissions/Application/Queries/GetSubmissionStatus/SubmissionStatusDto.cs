
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionStatus
{
    public sealed class SubmissionStatusDto
    {
        public Guid SubmissionId { get; init; }

        public SubmissionStatus Status { get; init; }
        public Verdict Verdict { get; init; }

        public int? CurrentTestCase { get; init; }
        public int? TotalTestCases { get; init; }

        public DateTime UpdatedAt { get; init; }
    }
}