namespace VAlgo.BuildingBlocks.Sandbox.Models
{
    public record class SandboxCompileRequest(
        string SourceCode,
        SandboxLanguage Language
    );
}