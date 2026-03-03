using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemForJudge;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Read
{
    public sealed class ProblemForJudgeQueries : IProblemForJudgeQueries
    {
        private readonly ProblemManagementDbContext _dbContext;

        public ProblemForJudgeQueries(ProblemManagementDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<ProblemForJudgeDto?> GetAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Problems
                .Where(x => x.Id == ProblemId.From(problemId) && x.Status == ProblemStatus.Published)
                .Select(x => new ProblemForJudgeDto
                {
                    ProblemId = x.Id.Value,
                    TimeLimitMs = x.TimeLimitMs,
                    MemoryLimitKb = x.MemoryLimitKb,
                    AllowedLanguages = x.AllowedLanguages.Select(a => a.Value).ToList(),
                    TestCases = x.TestCases.Select(tc => new JudgeTestCaseDto
                    {
                        Order = tc.Order,
                        Input = tc.Input,
                        ExpectedOutput = tc.ExpectedOutput,
                        ComparisonStrategy = tc.ComparisonStrategy
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}