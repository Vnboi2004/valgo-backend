namespace VAlgo.JudgeWorker.Execution.Models
{
    public enum Verdict
    {
        Accepted,
        WrongAnswer,
        TimeLimitExceeded,
        MemoryLimitExceeded,
        RuntimeError,
        CompilationError,
        SystemError
    }
}