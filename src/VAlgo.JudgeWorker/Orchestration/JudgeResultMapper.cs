using VAlgo.JudgeWorker.Execution.Models;

namespace VAlgo.JudgeWorker.Orchestration
{
    public sealed class JudgeResultMapper
    {
        public static Verdict FinalVerdict(IReadOnlyList<JudgeResult> results)
        {
            foreach (var r in results)
            {
                if (r.Verdict != Verdict.Accepted)
                    return r.Verdict;
            }

            return Verdict.Accepted;
        }
    }
}