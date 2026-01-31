using VAlgo.JudgeWorker.Clients.Models;
using VAlgo.JudgeWorker.Execution.Models;

namespace VAlgo.JudgeWorker.Execution.Comparators
{
    public interface IOutputComparator
    {
        Verdict Compare(string actualOutput, string expectedOutput);
        bool CanHandle(OutputComparisonStrategy strategy);
    }
}