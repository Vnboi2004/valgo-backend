using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyList;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyStats;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Read
{
    public sealed class CompanyReadRepository : ICompanyReadRepository
    {
        private readonly ProblemManagementDbContext _dbContext;

        public CompanyReadRepository(ProblemManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResult<CompanyListItemDto>> GetListAsync(bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Companies.AsNoTracking();

            if (isActive.HasValue)
                query = query.Where(x => x.IsActive == isActive.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(x => x.Name)
                .Skip((page - 1) * page)
                .Take(pageSize)
                .Select(x => new CompanyListItemDto
                {
                    CompanyId = x.Id.Value,
                    Name = x.Name,
                    IsActive = x.IsActive
                }).ToListAsync(cancellationToken);

            return new PagedResult<CompanyListItemDto>(items, totalCount, page, pageSize);
        }

        public async Task<CompanyDetailDto?> GetDetailAsync(Guid companyId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Companies
                .Where(x => x.Id.Value == companyId)
                .Select(x => new CompanyDetailDto
                {
                    CompanyId = x.Id.Value,
                    Name = x.Name,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<CompanyStatsDto>> GetCompanyStatsAsync(CancellationToken cancellationToken = default)
        {
            var companies = await _dbContext.Companies
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Select(c => new { c.Id, c.Name })
                .ToListAsync(cancellationToken);

            var companyIds = companies.Select(c => c.Id.Value).ToList();

            var problemCounts = await _dbContext.Set<ProblemCompanyRef>()
                .AsNoTracking()
                .Where(r => companyIds.Contains(r.CompanyId))
                .GroupBy(r => r.CompanyId)
                .Select(g => new { CompanyId = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);

            var countMap = problemCounts.ToDictionary(x => x.CompanyId, x => x.Count);

            return companies
                .Select(c => new CompanyStatsDto
                {
                    CompanyId = c.Id.Value,
                    Name = c.Name,
                    ProblemCount = countMap.TryGetValue(c.Id.Value, out var count) ? count : 0
                })
                .OrderByDescending(x => x.ProblemCount)
                .ToList();
        }
    }
}