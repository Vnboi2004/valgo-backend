namespace VAlgo.Modules.Submissions.Application.Abstractions
{
    public interface IJudgeQueue
    {
        Task EnqueueAsync(Guid submissionId, CancellationToken cancellationToken = default);
    }
}