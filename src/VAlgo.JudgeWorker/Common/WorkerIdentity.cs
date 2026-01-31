namespace VAlgo.JudgeWorker.Common
{
    public sealed class WorkerIdentity
    {
        public string WorkerId { get; }

        public WorkerIdentity()
            => WorkerId = $"judge-{Guid.NewGuid():N}";
    }
}