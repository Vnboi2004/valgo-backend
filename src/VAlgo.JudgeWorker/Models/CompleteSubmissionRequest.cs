namespace VAlgo.JudgeWorker.Models
{
    public sealed record CompleteSubmissionRequest(
        int TotalTestCases,
        int PassedTestCases,
        int MaxTimeMs,
        int MaxMemoryKb,
        Verdict Verdict,
        List<SubmissionTestCaseResult> TestCases
    );
}