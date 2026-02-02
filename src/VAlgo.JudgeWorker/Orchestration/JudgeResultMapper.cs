using VAlgo.JudgeWorker.Execution.Models;

namespace VAlgo.JudgeWorker.Orchestration
{
    public sealed class JudgeResultMapper
    {
        public JudgeSummary Map(IReadOnlyList<JudgeResult> results)
        {
            var total = results.Count;
            var passed = 0;
            var timeMs = 0;
            var memoryKb = 0;

            foreach (var r in results)
            {
                timeMs = Math.Max(timeMs, r.TimeMs);
                memoryKb = Math.Max(memoryKb, r.MemoryKb);

                if (r.Verdict != Verdict.Accepted)
                {
                    return new JudgeSummary
                    {
                        Verdict = r.Verdict,
                        Passed = passed,
                        Total = total,
                        TimeMs = timeMs,
                        MemoryKb = memoryKb
                    };
                }

                passed++;
            }

            return new JudgeSummary
            {
                Verdict = Verdict.Accepted,
                Passed = passed,
                Total = total,
                TimeMs = timeMs,
                MemoryKb = memoryKb
            };
        }
    }
}