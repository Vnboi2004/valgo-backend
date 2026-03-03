namespace VAlgo.Modules.Submissions.Application.Events
{
    public sealed record SubmissionQueuedIntegrationEvent(Guid SubmissionId, Guid ProblemId);
}