using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionStatus;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;

namespace VAlgo.Modules.Submissions.Infrastructure.Read
{
    public sealed class SubmissionStatusReadStore : ISubmissionStatusReadStore
    {
        private readonly SubmissionsDbContext _dbContext;

        public SubmissionStatusReadStore(SubmissionsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<SubmissionStatusDto?> GetStatusAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            return await _dbContext.Submissions
                .AsNoTracking()
                .Where(x => x.Id.Value == submissionId)
                .Select(x => new SubmissionStatusDto
                {
                    SubmissionId = x.Id.Value,
                    Status = x.Status,
                    Verdict = x.Verdict,
                    CurrentTestCase = null,
                    TotalTestCases = x.JudgeSummary != null ? x.JudgeSummary.TotalTestCases : null,
                    UpdatedAt = x.FinishedAt ?? x.StartedAt ?? x.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}