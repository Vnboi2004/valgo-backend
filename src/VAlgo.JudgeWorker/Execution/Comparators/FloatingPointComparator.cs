using System.Globalization;
using VAlgo.JudgeWorker.Clients.Models;
using VAlgo.JudgeWorker.Execution.Models;

namespace VAlgo.JudgeWorker.Execution.Comparators
{
    public sealed class FloatingPointComparator : IOutputComparator
    {
        private const double EPS = 1e-6;

        public bool CanHandle(OutputComparisonStrategy strategy)
            => strategy == OutputComparisonStrategy.FloatingPoint;

        public Verdict Compare(string actualOutput, string expectedOutput)
        {
            var actualTokens = Split(actualOutput);
            var expectedTokens = Split(expectedOutput);

            if (actualTokens.Length != expectedTokens.Length)
                return Verdict.WrongAnswer;

            for (int i = 0; i < actualTokens.Length; i++)
            {
                if (!double.TryParse(actualTokens[i], NumberStyles.Float, CultureInfo.InvariantCulture, out var actual))
                    return Verdict.WrongAnswer;

                if (!double.TryParse(expectedTokens[i], NumberStyles.Float, CultureInfo.InvariantCulture, out var expected))
                    return Verdict.WrongAnswer;

                if (!NearlyEqual(actual, expected))
                    return Verdict.WrongAnswer;
            }

            return Verdict.Accepted;
        }

        private static string[] Split(string input)
            => input
                .Trim()
                .Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        private static bool NearlyEqual(double a, double b)
        {
            var diff = Math.Abs(a - b);

            if (a == b)
                return true;

            return diff <= EPS * Math.Max(1.0, Math.Max(Math.Abs(a), Math.Abs(b)));
        }
    }
}