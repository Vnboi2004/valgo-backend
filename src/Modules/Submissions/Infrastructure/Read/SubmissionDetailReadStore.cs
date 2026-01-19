using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;

namespace VAlgo.Modules.Submissions.Infrastructure.Read
{
    public sealed class SubmissionDetailReadStore : ISubmissionDetailReadStore
    {
        private readonly SubmissionsDbContext _dbContext;

        public SubmissionDetailReadStore(SubmissionsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<SubmissionDetailDto?> GetByIdAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            return await _dbContext.Submissions
                .AsNoTracking()
                .Where(x => x.Id.Value == submissionId)
                .Select(x => new SubmissionDetailDto
                {
                    SubmissionId = x.Id.Value,
                    UserId = x.UserId,
                    ProblemId = x.ProblemId,
                    Language = x.Language.Value,
                    SourceCode = x.SourceCode,
                    Status = x.Status,
                    Verdict = x.Verdict,
                    Summary = x.JudgeSummary == null ? null : new JudgeSummaryDto
                    {
                        TotalTestCases = x.JudgeSummary.TotalTestCases,
                        PassedTestCases = x.JudgeSummary.PassedTestCases,
                        MaxTimeMs = x.JudgeSummary.MaxTimeMs,
                        MaxMemoryKb = x.JudgeSummary.MaxMemoryKb
                    },
                    CreatedAt = x.CreatedAt,
                    StartedAt = x.StartedAt,
                    FinishedAt = x.FinishedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}