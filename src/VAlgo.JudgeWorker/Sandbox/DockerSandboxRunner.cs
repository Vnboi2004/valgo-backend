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

        var containerName = $"sandbox-{Guid.NewGuid():N}";

        var dockerArgs =
            $"run --rm --name {containerName} " +
            $"--network none " +
            $"--pids-limit 64 " +
            $"--read-only " +
            $"--cap-drop ALL " +
            $"--security-opt no-new-privileges " +
            $"-v \"{workDir}:/sandbox\" " +
            $"-w /sandbox " +
            $"{request.Language.DockerImage} " +
            $"sh -c \"{request.Language.CompileCommand}\"";

        _logger.LogInformation("Docker compile args: {Args}", dockerArgs);

        var result = await ExecuteProcessAsync(
            "docker",
            dockerArgs,
            timeoutMs: 15_000,
            cancellationToken);

        if (result.ExitCode != 0)
        {
            _logger.LogWarning("Compile failed: {Error}", result.Stderr);
        }

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
        var containerName = $"sandbox-{Guid.NewGuid():N}";

        var dockerArgs =
            $"run --rm --name {containerName} " +
            $"--network none " +
            $"--memory {memoryMb}m " +
            $"--pids-limit 64 " +
            $"--read-only " +
            $"--cap-drop ALL " +
            $"--security-opt no-new-privileges " +
            $"-v \"{workDir}:/sandbox\" " +
            $"-w /sandbox " +
            $"{request.Language.DockerImage} " +
            $"sh -c \"/usr/bin/time -v {request.Language.RunCommand} < input.txt 2> time_result.txt\"";

        _logger.LogInformation("Docker args: {Args}", dockerArgs);


        var result = await ExecuteProcessAsync(
            "docker",
            dockerArgs,
            request.TimeLimitMs + 1000,
            cancellationToken);

        var timeMs = ParseTimeMs(workDir);
        var memoryKb = ParseMemoryKb(workDir);


        return new SandboxRunResult
        {
            ExitCode = result.ExitCode,
            Stdout = result.Stdout,
            Stderr = result.Stderr,
            TimeMs = timeMs,
            MemoryKb = memoryKb,
            Verdict = MapVerdict(result, timeMs, request.TimeLimitMs)
        };
    }

    private int ParseTimeMs(string workDir)
    {
        try
        {
            var timePath = Path.Combine(workDir, "time_result.txt");
            if (!File.Exists(timePath))
                return 0;

            var lines = File.ReadAllLines(timePath);

            double user = 0, sys = 0;

            foreach (var line in lines)
            {
                if (line.Contains("User time"))
                {
                    var value = line.Split(':').Last().Trim();
                    double.TryParse(value, System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out user);
                }

                if (line.Contains("System time"))
                {
                    var value = line.Split(':').Last().Trim();
                    double.TryParse(value, System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out sys);
                }
            }

            return (int)((user + sys) * 1000);
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to parse time usage: {Message}", ex.Message);
            return 0;
        }
    }

    private int ParseMemoryKb(string workDir)
    {
        try
        {
            var timePath = Path.Combine(workDir, "time_result.txt");
            if (!File.Exists(timePath))
                return 0;

            var lines = File.ReadAllLines(timePath);

            // /usr/bin/time -v output có dòng:
            // "Maximum resident set size (kbytes): 3456"
            foreach (var line in lines)
            {
                if (line.Contains("Maximum resident set size"))
                {
                    var parts = line.Split(':');
                    if (parts.Length == 2 && int.TryParse(parts[1].Trim(), out var kb))
                        return kb;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to parse memory usage: {Message}", ex.Message);
        }

        return 0;
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

        var containerName = $"sandbox-{Guid.NewGuid():N}";

        if (completed != exitTask)
        {
            TryKill(process);

            await ExecuteProcessAsync(
                "docker",
                $"kill {containerName}",
                5000,
                CancellationToken.None);

            return new ProcessResult
            {
                ExitCode = -1,
                Stdout = stdout.ToString(),
                Stderr = stderr.ToString()
            };
        }

        await exitTask;
        process.WaitForExit();

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
    private static Verdict MapVerdict(ProcessResult result, int timeMs, int timeLimitMs)
    {
        if (result.ExitCode == -1)
            return Verdict.TimeLimitExceeded;

        if (timeMs > timeLimitMs)
            return Verdict.TimeLimitExceeded;

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
