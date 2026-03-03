using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using VAlgo.JudgeWorker.Models;

namespace VAlgo.JudgeWorker.Sandbox;

public sealed class DockerSandboxRunner
{
    private readonly ILogger<DockerSandboxRunner> _logger;

    public DockerSandboxRunner(ILogger<DockerSandboxRunner> logger)
    {
        _logger = logger;
    }

    // ============================================================
    // COMPILE (run once)
    // ============================================================
    public async Task<CompileResult> CompileAsync(
        SandboxCompileRequest request,
        CancellationToken cancellationToken = default)
    {
        var workDir = CreateWorkDirectory();

        var sourcePath = Path.Combine(workDir, request.Language.SourceFileName);
        await File.WriteAllTextAsync(sourcePath, request.SourceCode, cancellationToken);

        var dockerArgs =
            $"run --rm " +
            $"-v \"{workDir}:/sandbox\" " +
            $"-w /sandbox " +
            $"{request.Language.DockerImage} " +
            $"sh -c \"{request.Language.CompileCommand}\"";

        var result = await ExecuteProcessAsync(
            "docker",
            dockerArgs,
            timeoutMs: 15_000,
            cancellationToken);

        return new CompileResult
        {
            Success = result.ExitCode == 0,
            Stderr = result.Stderr,
            WorkDir = workDir
        };
    }

    // ============================================================
    // RUN (per test case)
    // ============================================================
    public async Task<SandboxRunResult> RunAsync(
        SandboxRunRequest request,
        string workDir,
        CancellationToken cancellationToken = default)
    {
        var inputPath = Path.Combine(workDir, "input.txt");
        await File.WriteAllTextAsync(inputPath, request.Input, cancellationToken);

        var memoryMb = Math.Max(16, request.MemoryLimitKb / 1024);

        var dockerArgs =
            $"run --rm " +
            $"--network none " +
            $"--memory {memoryMb}m " +
            $"-v \"{workDir}:/sandbox\" " +
            $"-w /sandbox " +
            $"{request.Language.DockerImage} " +
            $"sh -c \"{request.Language.RunCommand} < input.txt\"";

        var stopwatch = Stopwatch.StartNew();

        var result = await ExecuteProcessAsync(
            "docker",
            dockerArgs,
            request.TimeLimitMs,
            cancellationToken);

        stopwatch.Stop();

        return new SandboxRunResult
        {
            ExitCode = result.ExitCode,
            Stdout = result.Stdout,
            Stderr = result.Stderr,
            TimeMs = (int)stopwatch.ElapsedMilliseconds,
            MemoryKb = 0,
            Verdict = MapVerdict(result)
        };
    }

    // ============================================================
    // PROCESS
    // ============================================================
    private async Task<ProcessResult> ExecuteProcessAsync(
        string fileName,
        string arguments,
        int timeoutMs,
        CancellationToken cancellationToken)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        var stdout = new StringBuilder();
        var stderr = new StringBuilder();

        process.OutputDataReceived += (_, e) =>
        {
            if (e.Data != null)
                stdout.AppendLine(e.Data);
        };

        process.ErrorDataReceived += (_, e) =>
        {
            if (e.Data != null)
                stderr.AppendLine(e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        var exitTask = process.WaitForExitAsync(cancellationToken);
        var completed = await Task.WhenAny(exitTask, Task.Delay(timeoutMs, cancellationToken));

        if (completed != exitTask)
        {
            TryKill(process);

            return new ProcessResult
            {
                ExitCode = -1,
                Stdout = stdout.ToString(),
                Stderr = stderr.ToString()
            };
        }

        return new ProcessResult
        {
            ExitCode = process.ExitCode,
            Stdout = stdout.ToString(),
            Stderr = stderr.ToString()
        };
    }

    // ============================================================
    // VERDICT
    // ============================================================
    private static Verdict MapVerdict(ProcessResult result)
    {
        if (result.ExitCode == -1)
            return Verdict.TimeLimitExceeded;

        if (!string.IsNullOrWhiteSpace(result.Stderr) && result.ExitCode != 0)
            return Verdict.CompileError;

        if (result.ExitCode == 137)
            return Verdict.MemoryLimitExceeded;

        if (result.ExitCode != 0)
            return Verdict.RuntimeError;

        return Verdict.Accepted;
    }

    // ============================================================
    // UTILS
    // ============================================================
    private static string CreateWorkDirectory()
    {
        var path = Path.Combine(
            Path.GetTempPath(),
            "valgo-sandbox",
            Guid.NewGuid().ToString("N"));

        Directory.CreateDirectory(path);
        return path;
    }

    public static void Cleanup(string path)
    {
        try
        {
            if (Directory.Exists(path))
                Directory.Delete(path, recursive: true);
        }
        catch { }
    }

    private static void TryKill(Process process)
    {
        try
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
        }
        catch { }
    }
}
