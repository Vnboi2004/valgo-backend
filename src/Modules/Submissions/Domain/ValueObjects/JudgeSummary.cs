using VAlgo.Modules.Submissions.Domain.Exceptions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.ValueObjects
{
    public sealed class JudgeSummary : ValueObject
    {
        public int TotalTestCases { get; private set; }
        public int PassedTestCases { get; private set; }
        public int MaxTimeMs { get; private set; }
        public int MaxMemoryKb { get; private set; }

        private JudgeSummary() { }

        private JudgeSummary(int total, int passed, int maxTimeMs, int maxMemoryKb)
        {
            if (total <= 0)
                throw new InvalidJudgeSummaryException("Total test cases must be greater than zero");

            if (passed < 0 || passed > total)
                throw new InvalidJudgeSummaryException("Invalid passed test cases");

            TotalTestCases = total;
            PassedTestCases = passed;
            MaxTimeMs = maxTimeMs;
            MaxMemoryKb = maxMemoryKb;
        }

        public static JudgeSummary Create(int total, int passed, int maxTimeMs, int maxMemoryKb)
            => new JudgeSummary(total, passed, maxTimeMs, maxMemoryKb);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return TotalTestCases;
            yield return PassedTestCases;
            yield return MaxTimeMs;
            yield return MaxMemoryKb;
        }
    }
}