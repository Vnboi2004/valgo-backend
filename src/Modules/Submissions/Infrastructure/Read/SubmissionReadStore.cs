using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissions;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;

namespace VAlgo.Modules.Submissions.Infrastructure.Read
{
    public sealed class SubmissionReadStore : ISubmissionReadStore
    {
        private readonly SubmissionsDbContext _dbContext;

        public SubmissionReadStore(SubmissionsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<(IReadOnlyList<SubmissionListItemDto> Items, int TotalCount)> GetSubmissionsAsync(
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
    }
}