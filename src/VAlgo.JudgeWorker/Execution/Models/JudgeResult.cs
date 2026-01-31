namespace VAlgo.JudgeWorker.Execution.Models
{
    public sealed class JudgeResult
    {
        public Verdict Verdict { get; init; }
        public string Stdout { get; init; } = string.Empty;
        public string Stderr { get; init; } = string.Empty;
        public int ExitCode { get; init; }
        public TimeSpan TimeUsed { get; init; }
    }
}