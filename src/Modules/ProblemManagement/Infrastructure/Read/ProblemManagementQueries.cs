using MediatR;
using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemCompanies;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemTags;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetSimilarProblems;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Read
{
    public sealed class ProblemManagementQueries : IProblemManagementQueries
    {
        private readonly ProblemManagementDbContext _dbContext;
        private readonly IMediator _mediator;

        public ProblemManagementQueries(ProblemManagementDbContext dbContext, IMediator mediator)
        {
            _dbContext = dbContext;
            _mediator = mediator;
        }

        public async Task<(IReadOnlyList<ProblemListItemDto> Items, int TotalCount)> GetListAsync(
            GetProblemListQuery filter,
            int skip,
            int take,
            CancellationToken cancellationToken = default
        )
        {
            var query = _dbContext.Problems
                .AsNoTracking()
                .Where(p => p.Status != ProblemStatus.Archived);

            // Keyword
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var keyword = filter.Keyword.Trim();

                query = query.Where(p => p.Title.Contains(keyword) || p.Code.Contains(keyword));
            }

            // Difficulty
            if (filter.Difficulty.HasValue)
                query = query.Where(p => p.Difficulty == filter.Difficulty);

            // Status
            if (filter.Status.HasValue)
                query = query.Where(p => p.Status == filter.Status);

            // Company filter
            if (filter.CompanyId.HasValue)
                query = query.Where(p => p.Companies.Any(c => c.CompanyId == filter.CompanyId));

            // Tag filter
            if (filter.ClassificationId.HasValue)
                query = query.Where(p => p.Classifications.Any(c => c.ClassificationId == filter.ClassificationId));

            // Sorting
            query = ApplySorting(query, filter);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip(skip)
                .Take(take)
                .Select(p => new ProblemListItemDto
                {
                    Id = p.Id.Value,
                    Code = p.Code,
                    Title = p.Title,
                    Difficulty = p.Difficulty,
                    Status = p.Status,
                    AcceptanceRate = 0
                }).ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<ProblemDetailDto?> GetDetailAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Problems
                .AsNoTracking()
                .Where(p =>
                    p.Id == ProblemId.From(problemId) &&
                    p.Status == ProblemStatus.Published)
                .Select(p => new ProblemDetailDto
                {
                    ProblemId = p.Id.Value,
                    Code = p.Code,
                    Title = p.Title,
                    Statement = p.Statement,

                    Difficulty = p.Difficulty,

                    TimeLimitMs = p.TimeLimitMs,
                    MemoryLimitKb = p.MemoryLimitKb,

                    Constraints = p.Constraints,
                    InputFormat = p.InputFormat,
                    OutputFormat = p.OutputFormat,

                    AllowedLanguages = p.AllowedLanguages
                        .Select(x => x.Value)
                        .ToList(),

                    Samples = p.TestCases
                        .Where(tc => tc.IsSample)
                        .OrderBy(tc => tc.Order)
                        .Select(tc => new ProblemSampleTestCaseDto
                        {
                            Order = tc.Order,
                            Input = tc.Input,
                            ExpectedOutput = tc.ExpectedOutput
                        }).ToList(),

                    Examples = p.Examples
                        .OrderBy(x => x.Order)
                        .Select(x => new ProblemExampleDto
                        {
                            Order = x.Order,
                            Input = x.Input,
                            Output = x.Output,
                            Explanation = x.Explanation
                        }).ToList(),

                    Hints = p.Hints
                        .OrderBy(x => x.Order)
                        .Select(x => x.Content)
                        .ToList(),

                    ClassificationIds = p.Classifications
                        .Select(x => x.ClassificationId)
                        .ToList(),

                    CompanyIds = p.Companies
                        .Select(x => x.CompanyId)
                        .ToList()
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

                    Constraints = p.Constraints,
                    InputFormat = p.InputFormat,
                    OutputFormat = p.OutputFormat,
                    FollowUp = p.FollowUp,
                    Editorial = p.Editorial,

                    Difficulty = p.Difficulty,
                    Status = p.Status,

                    TimeLimitMs = p.TimeLimitMs,
                    MemoryLimitKb = p.MemoryLimitKb,

                    AllowedLanguages = p.AllowedLanguages
                        .Select(x => x.Value)
                        .ToList(),

                    ClassificationIds = p.Classifications
                        .Select(x => x.ClassificationId)
                        .ToList(),

                    CompanyIds = p.Companies
                        .Select(x => x.CompanyId)
                        .ToList(),

                    SimilarProblemIds = p.SimilarProblems
                        .Select(x => x.ProblemId.Value)
                        .ToList(),

                    Examples = p.Examples
                        .OrderBy(x => x.Order)
                        .Select(x => new ProblemExampleEditorDto
                        {
                            ExampleId = x.Id.Value,
                            Order = x.Order,
                            Input = x.Input,
                            Output = x.Output,
                            Explanation = x.Explanation
                        }).ToList(),

                    Hints = p.Hints
                        .OrderBy(x => x.Order)
                        .Select(x => new ProblemHintEditorDto
                        {
                            HintId = x.Id.Value,
                            Order = x.Order,
                            Content = x.Content
                        }).ToList(),

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

        public async Task<IReadOnlyList<SimilarProblemDto>> GetSimilarProblemsAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Problems
                .AsNoTracking()
                .Where(p => p.Id.Value == problemId)
                .SelectMany(p => p.SimilarProblems)
                .Join(
                    _dbContext.Problems,
                    s => s.ProblemId.Value,
                    p => p.Id.Value,
                    (s, p) => new SimilarProblemDto
                    {
                        ProblemId = p.Id.Value,
                        Code = p.Code,
                        Title = p.Title,
                        Difficulty = p.Difficulty
                    })
                .Where(x => true)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ProblemCompanyDto>> GetProblemCompaniesAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Problems
                .AsNoTracking()
                .Where(p => p.Id.Value == problemId)
                .SelectMany(p => p.Companies)
                .Join(
                    _dbContext.Companies,
                    ref_ => ref_.CompanyId,
                    c => c.Id.Value,
                    (ref_, c) => new ProblemCompanyDto
                    {
                        CompanyId = c.Id.Value,
                        Name = c.Name
                    })
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ProblemTagDto>> GetProblemTagsAsync(Guid problemId, CancellationToken cancellationToken = default)
        {
            // Bước 1: EF — lấy classificationIds từ Problem DB (đúng module, đúng layer)
            var classificationIds = await _dbContext.Problems
                .AsNoTracking()
                .Where(p => p.Id.Value == problemId)
                .SelectMany(p => p.Classifications)
                .Select(c => c.ClassificationId)
                .ToListAsync(cancellationToken);

            if (!classificationIds.Any())
                return [];

            // Bước 2: cross-module — Classification module tự resolve Name từ DB của nó
            var query = new GetClassificationsByIdsQuery(classificationIds);
            var classifications = await _mediator.Send(query, cancellationToken);

            return classifications
                .Select(c => new ProblemTagDto
                {
                    ClassificationId = c.Id,
                    Name = c.Name,
                    Type = c.Type
                })
                .ToList();
        }

        private static IQueryable<Problem> ApplySorting(IQueryable<Problem> query, GetProblemListQuery filter)
        {
            return (filter.SortBy, filter.SortDirection) switch
            {
                (ProblemSortBy.Code, SortDirection.Asc) =>
                    query.OrderBy(x => x.Code),

                (ProblemSortBy.Code, SortDirection.Desc) =>
                    query.OrderByDescending(x => x.Code),

                (ProblemSortBy.Title, SortDirection.Asc) =>
                    query.OrderBy(x => x.Title),

                (ProblemSortBy.Title, SortDirection.Desc) =>
                    query.OrderByDescending(x => x.Title),

                (ProblemSortBy.Difficulty, SortDirection.Asc) =>
                    query.OrderBy(x => x.Difficulty),

                (ProblemSortBy.Difficulty, SortDirection.Desc) =>
                    query.OrderByDescending(x => x.Difficulty),

                _ => query.OrderBy(x => x.Code)
            };
        }
    }
}