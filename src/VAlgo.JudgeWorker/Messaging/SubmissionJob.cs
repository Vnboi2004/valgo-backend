namespace VAlgo.JudgeWorker.Messaging
{
    public sealed class SubmissionJob
    {
        public Guid SubmissionId { get; init; }

        public SubmissionJob(Guid submissionId)
            => SubmissionId = submissionId;
    }
}