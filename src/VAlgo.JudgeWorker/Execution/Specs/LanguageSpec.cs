namespace VAlgo.JudgeWorker.Execution.Specs
{
    public sealed class LanguageSpec
    {
        public required string Key { get; init; }
        public required string Image { get; init; }
        public required string SourceFile { get; init; }
        public required string? CompileCommand { get; init; }
        public required string RunCommand { get; init; }
    }
}