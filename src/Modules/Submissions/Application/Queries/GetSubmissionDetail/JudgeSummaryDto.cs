namespace VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail
{
    public sealed record JudgeSummaryDto
    {
        public int TotalTestCases { get; init; }
        public int PassedTestCases { get; init; }
        public int MaxTimeMs { get; init; }
        public int MaxMemoryKb { get; init; }
    }
}