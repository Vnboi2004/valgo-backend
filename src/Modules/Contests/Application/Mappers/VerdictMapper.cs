using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Contests.Application.Mappers
{
    public static class VerdictMapper
    {
        public static ContestSubmissionVerdict ToContestVerdict(Verdict verdict)
        {
            return verdict switch
            {
                Verdict.Accepted => ContestSubmissionVerdict.Accepted,
                Verdict.WrongAnswer => ContestSubmissionVerdict.WrongAnswer,
                Verdict.TimeLimitExceeded => ContestSubmissionVerdict.TimeLimitExceeded,
                Verdict.MemoryLimitExceeded => ContestSubmissionVerdict.MemoryLimitExceeded,
                Verdict.RuntimeError => ContestSubmissionVerdict.RuntimeError,
                Verdict.CompileError => ContestSubmissionVerdict.CompileError,

                Verdict.Node => throw new InvalidOperationException("Invalid verdict: None"),
                Verdict.SystemError => throw new InvalidOperationException("SystemError should not be processed in contest"),

                _ => throw new ArgumentOutOfRangeException(nameof(verdict), verdict, null)
            };
        }
    }
}