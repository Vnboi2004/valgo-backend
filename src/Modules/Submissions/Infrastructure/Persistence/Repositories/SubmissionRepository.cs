using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.Aggregates;
using VAlgo.Modules.Submissions.Domain.ValueObjects;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Repositories
{
    internal sealed class SubmissionRepository : ISubmissionRepository
    {
        private readonly SubmissionsDbContext _dbContext;

        public SubmissionRepository(SubmissionsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Submission submission, CancellationToken cancellationToken = default)
        {
            await _dbContext.Submissions.AddAsync(submission, cancellationToken);
        }
        public async Task<Submission?> GetByIdAsync(SubmissionId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Submissions.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }
    }
}