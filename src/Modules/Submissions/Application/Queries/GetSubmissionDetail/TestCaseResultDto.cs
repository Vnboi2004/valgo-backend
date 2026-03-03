namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail
{
    public sealed record TestCaseResultDto
    {
        public int Index { get; init; }
        public bool Passed { get; init; }
        public int TimeMs { get; init; }
        public int MemoryKb { get; init; }
    }
}