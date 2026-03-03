namespace VAlgo.JudgeWorker.Models
{
    public record class SandboxCompileRequest(
        string SourceCode,
        SandboxLanguage Language
    );
}