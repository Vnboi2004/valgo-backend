using MediatR;
using VAlgo.Modules.Submissions.Domain.Enums;
using VAlgo.SharedKernel.Abstractions;

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