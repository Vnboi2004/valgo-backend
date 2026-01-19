using VAlgo.Modules.Submissions.Application.Abstractions;

namespace VAlgo.Modules.Submissions.Infrastructure.Integration
{
    public sealed class JudgeQueue : IJudgeQueue
    {
        public Task EnqueueAsync(Guid submissionId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}