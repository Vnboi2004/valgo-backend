using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissions;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionStatus;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;

namespace VAlgo.Modules.Submissions.Infrastructure.Read
{
    public sealed class SubmissionQueries : ISubmissionQueries
    {
        private readonly SubmissionsDbContext _dbContext;

        public SubmissionQueries(SubmissionsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<(IReadOnlyList<SubmissionListItemDto> Items, int TotalCount)> GetListAsync(
            Guid? userId,
            Guid? problemId,
            int skip,
            int take,
            CancellationToken cancellationToken
        )
        {
            var query = _dbContext.Submissions.AsNoTracking();

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId);

            if (problemId.HasValue)
                query = query.Where(x => x.ProblemId == problemId);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(take)
                .Select(x => new SubmissionListItemDto
                {
                    SubmissionId = x.Id.Value,
                    UserId = x.UserId,
                    ProblemId = x.ProblemId,
                    Language = x.Language.Value,
                    Status = x.Status,
                    Verdict = x.Verdict,
                    PassedTestCases = x.JudgeSummary!.PassedTestCases,
                    TotalTestCases = x.JudgeSummary!.TotalTestCases,
                    CreatedAt = x.CreatedAt,
                    FinishedAt = x.FinishedAt
                }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<SubmissionDetailDto?> GetDetailAsync(Guid submissionId, CancellationToken cancellationToken)
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

        public async Task<IReadOnlyList<TestCaseResultDto>> GetTestCasesAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            return await _dbContext.Submissions
                .Select(s => new TestCaseResultDto
                {

                }).ToListAsync(cancellationToken);
        }
    }
}