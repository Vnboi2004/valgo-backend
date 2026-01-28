using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Read
{
    public sealed class ProblemManagementQueries : IProblemManagementQueries
    {
        private readonly ProblemManagementDbContext _dbContext;

        public ProblemManagementQueries(ProblemManagementDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<(IReadOnlyList<ProblemListItemDto> Items, int TotalCount)> GetListAsync(
            GetProblemListQuery filter,
            int skip,
            int take,
            CancellationToken cancellationToken = default
        )
        {
            var query = _dbContext.Problems.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                query = query.Where(p => p.Title.Contains(filter.Keyword) || p.Code.Contains(filter.Keyword));
            }

            if (filter.Difficulty.HasValue)
                query = query.Where(p => p.Difficulty == filter.Difficulty);

            if (filter.Status.HasValue)
                query = query.Where(p => p.Status == filter.Status);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(p => p.Code)
                .Skip(skip)
                .Take(take)
                .Select(p => new ProblemListItemDto
                {
                    Id = p.Id.Value,
                    Code = p.Code,
                    Title = p.Title,
                    Difficulty = p.Difficulty,
                    Status = p.Status
                }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<ProblemDetailDto?> GetDetailAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Problems
                .AsNoTracking()
                .Where(p => p.Id.Value == problemId && p.Status == ProblemStatus.Published)
                .Select(p => new ProblemDetailDto
                {
                    ProblemId = p.Id.Value,
                    Code = p.Code,
                    Title = p.Title,
                    Statement = p.Statement,
                    Difficulty = p.Difficulty,
                    TimeLimitMs = p.TimeLimitMs,
                    MemoryLimitKb = p.MemoryLimitKb,
                    AllowedLanguages = p.AllowedLanguages.Select(x => x.Value).ToList(),
                    Samples = p.TestCases
                        .Where(tc => tc.IsSample)
                        .OrderBy(tc => tc.Order)
                        .Select(tc => new ProblemSampleTestCaseDto
                        {
                            Order = tc.Order,
                            Input = tc.Input,
                            ExpectedOutput = tc.ExpectedOutput
                        }).ToList(),
                    ClassificationIds = p.Classifications.Select(x => x.ClassificationId).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProblemEditorDto?> GetEditorAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Problems
                .AsNoTracking()
                .Where(p => p.Id.Value == problemId)
                .Select(p => new ProblemEditorDto
                {
                    ProblemId = p.Id.Value,
                    Code = p.Code,
                    Title = p.Title,
                    Statement = p.Statement,
                    ShortDescription = p.ShortDescription,
                    Difficulty = p.Difficulty,
                    Status = p.Status,
                    TimeLimitMs = p.TimeLimitMs,
                    MemoryLimitKb = p.MemoryLimitKb,
                    AllowedLanguages = p.AllowedLanguages.Select(x => x.Value).ToList(),
                    ClassificationIds = p.Classifications.Select(x => x.ClassificationId).ToList(),
                    TestCases = p.TestCases
                        .OrderBy(tc => tc.Order)
                        .Select(tc => new ProblemEditorTestCaseDto
                        {
                            TestCaseId = tc.Id.Value,
                            Order = tc.Order,
                            Input = tc.Input,
                            ExpectedOutput = tc.ExpectedOutput,
                            ComparisonStrategy = tc.ComparisonStrategy,
                            IsSample = tc.IsSample
                        }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}