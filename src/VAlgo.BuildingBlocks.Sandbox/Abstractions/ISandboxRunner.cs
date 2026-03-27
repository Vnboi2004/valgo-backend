using VAlgo.BuildingBlocks.Sandbox.Models;

namespace VAlgo.BuildingBlocks.Sandbox.Abstractions
{
    public interface ISandboxRunner
    {
        Task<CompileResult> CompileAsync(SandboxCompileRequest request, CancellationToken cancellationToken = default);

        Task<SandboxRunResult> RunAsync(SandboxRunRequest request, string workDir, CancellationToken cancellationToken = default);
    }
}