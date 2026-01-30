using VAlgo.Modules.Submissions.Domain.Enums;
using VAlgo.Modules.Submissions.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.Entities
{
    public sealed class TestCaseResult : Entity<TestCaseResultId>
    {
        public int TestCaseIndex { get; private set; }
        public Verdict Verdict { get; private set; }
        public int TimeMs { get; private set; }
        public int MemoryKb { get; private set; }

        private TestCaseResult() { }

        private TestCaseResult(TestCaseResultId id, int testCaseIndex, Verdict verdict, int timeMs, int memoryKb) : base(id)
        {
            TestCaseIndex = testCaseIndex;
            Verdict = verdict;
            TimeMs = timeMs;
            MemoryKb = memoryKb;
        }

        public static TestCaseResult Create(int testCaseIndex, Verdict verdict, int timeMs, int memoryKb)
            => new TestCaseResult(TestCaseResultId.New(), testCaseIndex, verdict, timeMs, memoryKb);
    }
}