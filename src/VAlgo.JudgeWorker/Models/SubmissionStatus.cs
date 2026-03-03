namespace VAlgo.JudgeWorker.Models
{
    public enum SubmissionStatus
    {
        Created = 0,
        Queued = 1,
        Running = 2,
        Completed = 3,
        Failed = 4,
        Cancelled = 5
    }
}