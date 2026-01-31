namespace VAlgo.JudgeWorker.Messaging
{
    public interface IJobQueue
    {
        Task<SubmissionJob> DequeueAsync(CancellationToken cancellationToken);
    }
}