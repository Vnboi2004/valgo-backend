using System.Collections.Concurrent;

namespace VAlgo.JudgeWorker.Messaging
{
    public sealed class InMemoryJobQueue : IJobQueue
    {
        private readonly ConcurrentQueue<SubmissionJob> _queue = new();

        public Task<SubmissionJob> DequeueAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out var job))
                    return Task.FromResult(job);

                Thread.Sleep(200);
            }

            throw new OperationCanceledException();
        }

        public void Enqueue(Guid submissionId)
            => _queue.Enqueue(new SubmissionJob(submissionId));
    }
}