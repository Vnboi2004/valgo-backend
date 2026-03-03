namespace VAlgo.JudgeWorker.Models
{
    public enum SubmissionFailureReason
    {
        Unknown = 0,
        WorkerUnavailable = 1,
        SandboxError = 2,
        CompileTimeout = 3,
        RuntimeCrash = 4,
        ResourceLimitExceeded = 5,
        InternalJudgeError = 100
    }
}