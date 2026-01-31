using System.Text.RegularExpressions;
using VAlgo.JudgeWorker.Clients.Models;
using VAlgo.JudgeWorker.Execution.Models;

namespace VAlgo.JudgeWorker.Execution.Comparators
{
    public sealed class IgnoreWhitespaceComparator : IOutputComparator
    {
        public bool CanHandle(OutputComparisonStrategy strategy)
            => strategy == OutputComparisonStrategy.IgnoreWhitespace;

        public Verdict Compare(string actualOutput, string expectedOutput)
        {
            var actualTokens = Normalize(actualOutput);
            var expectedTokens = Normalize(expectedOutput);

            if (actualTokens.Length != expectedTokens.Length)
                return Verdict.WrongAnswer;

            for (int i = 0; i < actualTokens.Length; i++)
            {
                if (!string.Equals(actualTokens[i], expectedTokens[i], StringComparison.Ordinal))
                    return Verdict.WrongAnswer;
            }

            return Verdict.Accepted;
        }

        private static string[] Normalize(string input)
        {
            return Regex
                .Split(input.Trim(), @"\s+")
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToArray();
        }
    }
}