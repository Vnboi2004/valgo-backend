using VAlgo.JudgeWorker.Clients.Models;
using VAlgo.JudgeWorker.Execution.Models;

namespace VAlgo.JudgeWorker.Execution.Comparators
{
    public sealed class ExactComparator : IOutputComparator
    {
        public bool CanHandle(OutputComparisonStrategy strategy)
            => strategy == OutputComparisonStrategy.Exact;

        public Verdict Compare(string actualOutput, string expectedOutput)
        {
            return actualOutput == expectedOutput
                ? Verdict.Accepted
                : Verdict.WrongAnswer;
        }
    }
}