using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;
using VAlgo.Modules.Submissions.Application.Abstractions;

namespace VAlgo.API.Services
{
    public sealed class ProblemReadService : IProblemReadService
    {
        private readonly ProblemManagementDbContext _dbContext;

        public ProblemReadService(ProblemManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(Guid problemId)
        {
            return await _dbContext.Problems
                .AsNoTracking()
                .AnyAsync(x => x.Id == ProblemId.From(problemId));
        }
    }
}