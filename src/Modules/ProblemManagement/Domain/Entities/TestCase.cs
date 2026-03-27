using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Domain.Entities
{
    public sealed class TestCase : Entity<TestCaseId>
    {
        public int Order { get; private set; }
        public string Input { get; private set; } = null!;
        public string ExpectedOutput { get; private set; } = null!;
        public OutputComparisonStrategy ComparisonStrategy { get; private set; }
        public bool IsSample { get; private set; }

        private TestCase() { }

        internal TestCase(
            TestCaseId id,
            int order,
            string input,
            string expectedOutput,
            OutputComparisonStrategy comparisonStrategy,
            bool isSample
        ) : base(id)
        {
            Order = order;
            Input = input;
            ExpectedOutput = expectedOutput;
            ComparisonStrategy = comparisonStrategy;
            IsSample = isSample;
        }

        internal static TestCase Create(
            int order,
            string input,
            string expectedOutput,
            OutputComparisonStrategy comparisonStrategy,
            bool isSample
        )
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new InvalidTestCaseException("Input is empty");

            if (expectedOutput == null)
                throw new InvalidTestCaseException("Expected output is null");

            return new TestCase(TestCaseId.New(), order, input, expectedOutput, comparisonStrategy, isSample);
        }

        public void SetOrder(int order)
        {
            Order = order;
        }

        public void Update(string input, string expectedOutput, OutputComparisonStrategy comparisonStrategy, bool isSample)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new InvalidTestCaseException("Input is empty.");

            if (expectedOutput == null)
                throw new InvalidTestCaseException("Expected output is null.");

            Input = input;
            ExpectedOutput = expectedOutput;
            ComparisonStrategy = comparisonStrategy;
            IsSample = isSample;
        }
    }
}