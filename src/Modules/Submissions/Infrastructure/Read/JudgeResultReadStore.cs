using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Application.Queries.GetSubmissionDetail;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;

namespace VAlgo.Modules.Submissions.Infrastructure.Read
{
    public sealed class JudgeResultReadStore : IJudgeResultReadStore
    {
        private readonly SubmissionsDbContext _dbContext;

        public JudgeResultReadStore(SubmissionsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<IReadOnlyList<TestCaseResultDto>> GetTestCasesAsync(Guid submissionId, CancellationToken cancellationToken)
        {
            return await _dbContext.TestCaseResults
                .AsNoTracking()
                .Where(x => x.SubmissionId == submissionId)
                .OrderBy(x => x.Index)
                .Select(x => new TestCaseResultDto
                {
                    Index = x.Index,
                    Passed = x.Passed,
                    TimeMs = x.TimeMs,
                    MemoryKb = x.MemoryKb,
                    Error = x.Error
                }).ToListAsync(cancellationToken);
        }
    }
}