using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.BuildingBlocks.Sandbox.Models
{
    public sealed class SandboxRunResult
    {
        public int ExitCode { get; init; }
        public string Stdout { get; init; } = string.Empty;
        public string Stderr { get; init; } = string.Empty;
        public int TimeMs { get; init; }
        public int MemoryKb { get; init; }
        public Verdict Verdict { get; init; }
    }
}