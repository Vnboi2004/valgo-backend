using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Contests.Application.DTOs;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;

namespace VAlgo.Modules.Submissions.Infrastructure.ReadServices
{
    public sealed class SubmissionReadService : ISubmissionReadService
    {
        private readonly SubmissionsDbContext _dbContext;

        public SubmissionReadService(SubmissionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<SubmissionDto>> GetByContestIdAsync(
            Guid contestId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Submissions
                .Where(x => x.ContestId == contestId)
                .Select(x => new SubmissionDto
                {
                    UserId = x.UserId,
                    ProblemId = x.ProblemId,
                    Verdict = x.Verdict,
                })
                .ToListAsync(cancellationToken);
        }
    }
}