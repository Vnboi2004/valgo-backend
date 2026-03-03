namespace VAlgo.JudgeWorker.Models
{
    public sealed record SandboxLanguage(
        string Name,
        string DockerImage,
        string SourceFileName,
        string RunCommand,
        string CompileCommand
    );
}