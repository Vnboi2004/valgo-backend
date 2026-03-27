using MediatR;
using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;

namespace VAlgo.Modules.Submissions.Infrastructure.Integration
{
    public sealed class ProblemReadToSubmissionService : IProblemReadToSubmissionService
    {
        private readonly SubmissionsDbContext _dbContext;

        public ProblemReadToSubmissionService(SubmissionsDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<bool> ExistsAsync(Guid problemId)
        {
            return await _dbContext.Submissions
                .AsNoTracking()
                .AnyAsync(x => x.ProblemId == problemId);
        }
    }
}