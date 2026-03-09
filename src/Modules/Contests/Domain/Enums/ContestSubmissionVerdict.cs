namespace VAlgo.Modules.Contests.Domain.Enums
{
    public enum ContestSubmissionVerdict
    {
        Accepted = 1,
        WrongAnswer = 2,
        TimeLimitExceeded = 3,
        MemoryLimitExceeded = 4,
        RuntimeError = 5,
        CompileError = 6
    }
}