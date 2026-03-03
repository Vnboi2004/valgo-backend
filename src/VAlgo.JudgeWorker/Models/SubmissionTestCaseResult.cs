namespace VAlgo.JudgeWorker.Models
{
    public sealed record SubmissionTestCaseResult(
         int Index,
        Verdict Verdict,
        int TimeMs,
        int MemoryKb,
        string? Output
    );
}