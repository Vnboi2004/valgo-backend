using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.JudgeWorker.Models
{
    public sealed record FailSubmissionRequest(SubmissionFailureReason Reason);
}