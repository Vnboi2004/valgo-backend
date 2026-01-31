namespace VAlgo.JudgeWorker.Models
{
    public sealed class JudgeOutcome
    {
        public string Verdict { get; init; } = default!;
        public int Passed { get; init; }
        public int Total { get; init; }
        public long TimeMs { get; init; }
        public long MemoryKb { get; init; }
    }
}