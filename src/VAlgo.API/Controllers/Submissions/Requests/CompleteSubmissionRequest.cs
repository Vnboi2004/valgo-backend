using VAlgo.Modules.Submissions.Application.Commands.CompleteSubmission;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.API.Controllers.Submissions.Requests
{
    public sealed record CompleteSubmissionRequest(
        Verdict Verdict,
        int PassedTestCases,
        int TotalTestCases,
        int TimeMs,
        int MemoryKb,
        List<TestCaseResultDto> TestCases
    );
}