using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.SharedKernel.Contracts.Events
{
    public class SubmissionResultIntegrationEvent
    {
        public Guid SubmissionId { get; init; }
        public Guid UserId { get; init; }
        public Guid ProblemId { get; init; }
        public Guid? ContestId { get; init; }
        public Verdict Verdict { get; init; }
        public DateTime SubmittedAt { get; init; }
    }
}