namespace VAlgo.JudgeWorker.Execution.Specs
{
    public sealed class LanguageSpec
    {
        public string Image { get; init; } = null!;
        public string SourceFile { get; init; } = null!;
        public string? CompileCommand { get; init; }
        public string RunCommand { get; init; } = null!;
        public int TimeMultiplier { get; init; } = 1;
    }
}