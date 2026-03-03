namespace VAlgo.JudgeWorker.Models
{
    public sealed record SandboxRunRequest(
        string SourceCode,
        string Input,
        SandboxLanguage Language,
        int TimeLimitMs,
        int MemoryLimitKb
    );
}