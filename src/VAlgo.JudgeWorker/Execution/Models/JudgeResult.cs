namespace VAlgo.JudgeWorker.Execution.Models
{
    public sealed class JudgeResult
    {
        public Verdict Verdict { get; init; }
        public string? Output { get; init; }
        public string? Error { get; init; }
        public int TimeMs { get; init; }
        public int MemoryKb { get; init; }

        public static JudgeResult Accepted(string output, int timeMs, int memoryKb) => new()
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