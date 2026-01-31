using VAlgo.JudgeWorker.Execution.Docker;
using VAlgo.JudgeWorker.Execution.Models;

namespace VAlgo.JudgeWorker.Orchestration
{
    public sealed class JudgeOrchestrator
    {
        private readonly DockerExecutor _executor;

        public JudgeOrchestrator(DockerExecutor executor)
        {
            _executor = executor;
        }

        public IReadOnlyList<JudgeResult> Execute(JudgeContext context)
        {
            var results = new List<JudgeResult>();

            foreach (var testCase in context.Problem.TestCases)
            {
                var result = _executor.Execute(
                    context.LanguageSpec,
                    context.Submission.SourceCode,
                    testCase.Input,
                    context.TimeLimitMs,
                    context.MemoryLimitKb
                );

                results.Add(result);

                if (result.Verdict != Verdict.Accepted)
                    break;
            }

            return results;
        }
    }
}