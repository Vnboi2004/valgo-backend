using VAlgo.Modules.Submissions.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Submissions.Domain.Entities
{
    public sealed class TestCaseResult : Entity<TestCaseResultId>
    {
        public SubmissionId SubmissionId { get; private set; } = null!;
        public int TestCaseIndex { get; private set; }
        public Verdict Verdict { get; private set; }
        public int TimeMs { get; private set; }
        public int MemoryKb { get; private set; }
        public string? Output { get; private set; }

        private TestCaseResult() { }

        private TestCaseResult(TestCaseResultId id, SubmissionId submissionId, int testCaseIndex, Verdict verdict, int timeMs, int memoryKb, string? output)
            : base(id)
        {
            SubmissionId = submissionId;
            TestCaseIndex = testCaseIndex;
            Verdict = verdict;
            TimeMs = timeMs;
            MemoryKb = memoryKb;
            Output = output;
        }

        public static TestCaseResult Create(SubmissionId submissionId, int testCaseIndex, Verdict verdict, int timeMs, int memoryKb, string? output)
            => new TestCaseResult(TestCaseResultId.New(), submissionId, testCaseIndex, verdict, timeMs, memoryKb, output);
    }
}