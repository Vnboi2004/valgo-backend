using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Repositories
{
    public sealed class ProblemRepository : IProblemRepository
    {
        private readonly ProblemManagementDbContext _dbContext;

        public ProblemRepository(ProblemManagementDbContext dbContext) => _dbContext = dbContext;

        public async Task AddAsync(Problem problem, CancellationToken cancellationToken = default)
        {
            await _dbContext.Problems.AddAsync(problem, cancellationToken);
        }

        public async Task UpdateAsync(Problem problem, CancellationToken cancellationToken = default)
        {
            _dbContext.Problems.Update(problem);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Problems.AnyAsync(x => x.Code == code, cancellationToken);
        }

        public async Task<Problem?> GetByIdAsync(ProblemId id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Problems
                .Include(x => x.TestCases)
                .Include(x => x.Classifications)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}