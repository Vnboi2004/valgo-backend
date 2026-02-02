using VAlgo.JudgeWorker.Clients.Models;
using VAlgo.JudgeWorker.Execution.Specs;
using VAlgo.JudgeWorker.Languages;

namespace VAlgo.JudgeWorker.Orchestration
{
    public sealed class JudgeContext
    {
        public required SubmissionDto Submission { get; init; }
        public required ProblemForJudgeDto Problem { get; init; }
        public required LanguageSpec LanguageSpec { get; init; }

        public int TimeLimitMs => Problem.TimeLimitMs;
        public int MemoryLimitKb => Problem.MemoryLimitKb;

        public static JudgeContext Create(SubmissionDto submission, ProblemForJudgeDto problem)
        {
            return new JudgeContext
            {
                Submission = submission,
                Problem = problem,
                LanguageSpec = LanguageRegistry.Get(submission.Language)
            };
        }
    }
}