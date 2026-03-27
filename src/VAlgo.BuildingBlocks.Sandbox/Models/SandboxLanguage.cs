namespace VAlgo.BuildingBlocks.Sandbox.Models
{
    public sealed record SandboxLanguage(
        string Name,
        string DockerImage,
        string SourceFileName,
        string RunCommand,
        string CompileCommand
    );
}