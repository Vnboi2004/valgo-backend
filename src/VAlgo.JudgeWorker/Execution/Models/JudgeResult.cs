namespace VAlgo.JudgeWorker.Execution.Models
{
    public sealed class JudgeResult
    {
        public Verdict Verdict { get; init; }
        public string? Output { get; init; }
        public string? Error { get; init; }
        public long TimeMs { get; init; }
        public long MemoryKb { get; init; }

        public static JudgeResult Accepted(string output, long timeMs, long memoryKb) => new()
        {
            Verdict = Verdict.Accepted,
            Output = output,
            TimeMs = timeMs,
            MemoryKb = memoryKb
        };

        public static JudgeResult Failed(Verdict verdict, string? error = null) => new()
        {
            Verdict = verdict,
            Error = error
        };
    }
}