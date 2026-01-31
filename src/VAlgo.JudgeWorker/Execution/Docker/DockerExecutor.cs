using System.Diagnostics;
using VAlgo.JudgeWorker.Execution.Models;
using VAlgo.JudgeWorker.Execution.Specs;

namespace VAlgo.JudgeWorker.Execution.Docker
{
    public sealed class DockerExecutor
    {
        public ExecutionResult Execute(
            LanguageSpec spec,
            string sourceCode,
            string input,
            int timeLimitMs,
            int memoryLimitMb)
        {
            var workDir = CreateWorkDir(spec, sourceCode, input);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // 1. Compile (id needed)
                if (!string.IsNullOrWhiteSpace(spec.CompileCommand))
                {
                    var compile = RunDocker(spec.Image, workDir, spec.CompileCommand, 10_000, memoryLimitMb);

                    if (compile.ExitCode != 0)
                    {
                        return new ExecutionResult
                        {
                            Verdict = Verdict.CompilationError,
                            Stdout = ReadFile(workDir, "compile.out"),
                            Stderr = ReadFile(workDir, "compile.err"),
                            ExitCode = compile.ExitCode,
                            TimeUsed = stopwatch.Elapsed
                        };
                    }
                }

                // 2. Calculate time
                var baseTimeSec = Math.Max(1, timeLimitMs / 1000);
                var timeSec = baseTimeSec * spec.TimeMultiplier;
                var runCommand = spec.RunCommand
                    .Replace("{timeSec}", timeSec.ToString())
                    .Replace("{timeSec2}", (timeSec * 2).ToString())
                    .Replace("{memMb}", memoryLimitMb.ToString());

                // 3. Run
                var run = RunDocker(
                    spec.Image,
                    workDir,
                    runCommand,
                    timeLimitMs,
                    memoryLimitMb
                );

                stopwatch.Stop();

                var stdout = ReadFile(workDir, "stdout.txt");
                var stderr = ReadFile(workDir, "stderr.txt");

                // 3. Map exit code -> verdict 
                if (run.ExitCode == 124)
                    return Result(Verdict.TimeLimitExceeded, stdout, stderr, run, stopwatch);

                if (run.ExitCode == 137)
                    return Result(Verdict.MemoryLimitExceeded, stdout, stderr, run, stopwatch);

                if (run.ExitCode != 0)
                    return Result(Verdict.RuntimeError, stdout, stderr, run, stopwatch);

                return Result(Verdict.Accepted, stdout, stderr, run, stopwatch);
            }
            catch (Exception ex)
            {
                return new ExecutionResult
                {
                    Verdict = Verdict.SystemError,
                    Stderr = ex.ToString(),
                    TimeUsed = stopwatch.Elapsed
                };
            }
            finally
            {
                TryDelete(workDir);
            }
        }

        private static string CreateWorkDir(LanguageSpec spec, string source, string input)
        {
            var dir = Path.Combine(Path.GetTempPath(), "judge_" + Guid.NewGuid());
            Directory.CreateDirectory(dir);

            File.WriteAllText(Path.Combine(dir, spec.SourceFile), source);
            File.WriteAllText(Path.Combine(dir, "input.txt"), source);

            return dir;
        }

        private static ProcessResult RunDocker(string image, string workDir, string command, int timeoutMs, int memoryLimitMb)
        {
            var args = $"""
                run --rm
                --cpus=1
                --memory={memoryLimitMb}m
                --pids-limit=64
                --network=none
                --read-only
                --tmpfs /tmp:size=64m
                -v "{workDir}:/work"
                -w /work
                {image}
                bash -c "{command}"
            """;

            return RunProcess("docker", args, timeoutMs + 1000);
        }

        private static ProcessResult RunProcess(string file, string args, int timeoutMs)
        {
            var psi = new ProcessStartInfo
            {
                FileName = file,
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi)!;

            if (!process.WaitForExit(timeoutMs))
            {
                try
                {
                    process.Kill(true);
                }
                catch
                {

                }

                return new ProcessResult
                {
                    ExitCode = 124
                };
            }

            return new ProcessResult
            {
                ExitCode = process.ExitCode
            };
        }

        private static ExecutionResult Result(Verdict verdict, string stdout, string stderr, ProcessResult process, Stopwatch stopwatch) => new()
        {
            Verdict = verdict,
            Stdout = stdout,
            Stderr = stderr,
            ExitCode = process.ExitCode,
            TimeUsed = stopwatch.Elapsed
        };

        private static string ReadFile(string dir, string file)
        {
            var path = Path.Combine(dir, file);
            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
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

        private sealed class ProcessResult
        {
            public int ExitCode { get; init; }
        }
    }
}