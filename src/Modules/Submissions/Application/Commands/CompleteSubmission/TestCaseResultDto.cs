
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Application.Commands.CompleteSubmission
{
    public record TestCaseResultDto(
        int Index,
        Verdict Verdict,
        int TimeMs,
        int MemoryKb,
        string? Output
    );
}