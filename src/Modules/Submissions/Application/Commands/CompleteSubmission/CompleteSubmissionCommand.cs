using MediatR;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Application.Commands.CompleteSubmission
{
    public sealed record CompleteSubmissionCommand(
        Guid SubmissionId,
        Verdict Verdict,
        int PassedTestCases,
        int TotalTestCases,
        int TimeMs,
        int MemoryKb,
        List<TestCaseResultDto> TestCases
    ) : ICommand<Unit>;
}