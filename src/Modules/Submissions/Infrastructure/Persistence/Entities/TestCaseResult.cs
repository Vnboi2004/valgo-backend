namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Entities
{
    public sealed class TestCaseResult
    {
        public Guid Id { get; private set; }
        public Guid SubmissionId { get; private set; }
        public int Index { get; private set; }
        public bool Passed { get; private set; }
        public int TimeMs { get; private set; }
        public int MemoryKb { get; private set; }
        public string? Error { get; private set; }
        public DateTime CreatedAt { get; private set; }
    }
}