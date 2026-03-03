namespace VAlgo.JudgeWorker.Models
{
    public sealed record JudgeJobMessage(
        Guid SubmissionId,
        Guid ProblemId
    );
}