using VAlgo.Modules.Submissions.Domain.Enums;

namespace VAlgo.API.Controllers.Submissions.Requests
{
    public sealed record CompleteSubmissionRequest(
        Verdict Verdict,
        int PassedTestCases,
        int TotalTestCases,
        int TimeMs,
        int MemoryKb
    );
}