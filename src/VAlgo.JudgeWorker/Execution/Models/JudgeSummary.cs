namespace VAlgo.JudgeWorker.Execution.Models
{
    public sealed class JudgeSummary
    {
        public Verdict Verdict { get; init; }
        public int Passed { get; init; }
        public int Total { get; init; }
        public int TimeMs { get; init; }
        public int MemoryKb { get; init; }
    }
}