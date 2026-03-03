namespace VAlgo.JudgeWorker.Models
{
    public enum Verdict
    {
        Node = 0,

        Accepted = 1,
        WrongAnswer = 2,
        TimeLimitExceeded = 3,
        MemoryLimitExceeded = 4,
        RuntimeError = 5,
        CompileError = 6,
        SystemError = 100
    }
}