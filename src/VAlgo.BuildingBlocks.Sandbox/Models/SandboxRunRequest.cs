namespace VAlgo.BuildingBlocks.Sandbox.Models
{
    public sealed record SandboxRunRequest(
        string SourceCode,
        string Input,
        SandboxLanguage Language,
        int TimeLimitMs,
        int MemoryLimitKb
    );
}