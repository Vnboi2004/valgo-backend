using System.Diagnostics;
using System.Text;
using VAlgo.JudgeWorker.Execution.Models;
using VAlgo.JudgeWorker.Execution.Specs;

namespace VAlgo.JudgeWorker.Execution.Docker
{
    public sealed class DockerExecutor
    {

        public JudgeResult Execute(
            LanguageSpec spec,
            string sourceCode,
            string input,
            int timeLimitMs,
            int memoryLimitKb
        )
        {
            var workDir = CreateWorkDir();

            try
            {
                WriteSource(workDir, spec.SourceFile, sourceCode);

                if (!string.IsNullOrWhiteSpace(spec.CompileCommand))
                {
                    var compileResult = RunDocker(spec.Image, workDir, spec.CompileCommand!, timeLimitMs, memoryLimitKb);

                    if (compileResult.IsSystemError)
                        return JudgeResult.Failed(Verdict.SystemError, compileResult.Error);

                    if (compileResult.ExitCode != 0)
                        return JudgeResult.Failed(Verdict.CompilationError, compileResult.Error);
                }

                var timeSec = Math.Max(1, timeLimitMs / 1000);
                var memMb = Math.Max(64, memoryLimitKb / 1024);

                var runCommand = spec.RunCommand
                    .Replace("{timeSec}", timeSec.ToString())
                    .Replace("{timeSec2}", (timeSec * 2).ToString())
                    .Replace("{memMb}", memMb.ToString());

                var runResult = RunDocker(spec.Image, workDir, runCommand, timeLimitMs, memoryLimitKb, input);

                return runResult.ExitCode switch
                {
                    0 => JudgeResult.Accepted(runResult.Output ?? string.Empty, runResult.TimeMs, runResult.MemoryKb),
                    124 => JudgeResult.Failed(Verdict.TimeLimitExceeded),
                    123 => JudgeResult.Failed(Verdict.MemoryLimitExceeded),
                    _ => JudgeResult.Failed(Verdict.RuntimeError, runResult.Error)
                };
            }
            catch (Exception ex)
            {
                return JudgeResult.Failed(Verdict.SystemError, ex.Message);
            }
            finally
            {
                TryDelete(workDir);
            }
        }

        private static DockerProcessResult RunDocker(
            string image,
            string workDir,
            string command,
            int timeLimitMs,
            int memoryLimitKb,
            string? stdin = null
        )
        {
            var memMb = Math.Max(1, memoryLimitKb / 1024);
            var timeoutMs = timeLimitMs + 500;

            var args = $"""
                run --rm
                --cpus=1
                --memory={memMb}m
                --pids-limit=64
                --network=none
                --read-only
                --tmpfs /tmp:size=64m
                --cap-drop=ALL
                --security-opt no-new-privileges
                --user 1000:1000
                -v "{workDir}:/work"
                -w /work
                {image}
                sh -c "{command}"
            """;

            var psi = new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = args,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            var sw = Stopwatch.StartNew();

            try
            {
                using var process = Process.Start(psi)!;

                if (!string.IsNullOrEmpty(stdin))
                {
                    process.StandardInput.Write(stdin);
                    process.StandardInput.Close();
                }

                if (!process.WaitForExit(timeoutMs))
                {
                    TryKill(process);
                    return DockerProcessResult.Timeout();
                }

                sw.Stop();

                return new DockerProcessResult
                {
                    ExitCode = process.ExitCode,
                    Output = process.StandardOutput.ReadToEnd(),
                    Error = process.StandardError.ReadToEnd(),
                    TimeMs = sw.ElapsedMilliseconds,
                    MemoryKb = memMb * 1024
                };
            }
            catch (Exception ex)
            {
                return DockerProcessResult.SystemError(ex.Message);
            }
        }

        private static string CreateWorkDir()
        {
            var dir = Path.Combine(Path.GetTempPath(), "valgo-judge", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static void WriteSource(string dir, string file, string code)
        {
            File.WriteAllText(Path.Combine(dir, file), code, Encoding.UTF8);
        }

        private static void TryDelete(string dir)
        {
            try
            {
                Directory.Delete(dir, true);
            }
            catch
            {

            }
        }

        private static void TryKill(Process p)
        {
            try
            {
                p.Kill(true);
            }
            catch
            {

            }
        }

        internal sealed class DockerProcessResult
        {
            public int ExitCode { get; init; }
            public string? Output { get; init; }
            public string? Error { get; init; }
            public long TimeMs { get; init; }
            public long MemoryKb { get; init; }
            public bool IsSystemError { get; init; }

            public static DockerProcessResult Timeout()
                => new() { ExitCode = 124 };

            public static DockerProcessResult SystemError(string error)
                => new() { ExitCode = -1, Error = error, IsSystemError = true };
        }
    }
}