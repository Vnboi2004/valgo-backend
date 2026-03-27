namespace VAlgo.BuildingBlocks.Sandbox.Models
{
    public sealed class CompileResult
    {
        public bool Success { get; set; }
        public string? Stderr { get; set; }
        public string WorkDir { get; set; } = default!;
    }
}