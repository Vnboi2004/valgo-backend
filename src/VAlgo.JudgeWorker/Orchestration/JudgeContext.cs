using VAlgo.JudgeWorker.Clients.Models;
using VAlgo.JudgeWorker.Execution.Specs;

namespace VAlgo.JudgeWorker.Orchestration
{
    public sealed class JudgeContext
    {
        public required SubmissionDto Submission { get; init; }
        public required ProblemForJudgeDto Problem { get; init; }
        public required LanguageSpec LanguageSpec { get; init; }

        public int TimeLimitMs => Problem.TimeLimitMs;
        public int MemoryLimitKb => Problem.MemoryLimitKb / 1024;
    }
}