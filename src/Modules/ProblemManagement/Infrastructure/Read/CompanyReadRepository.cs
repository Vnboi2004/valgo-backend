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
            // Cùng DbContext nên có thể join trực tiếp
            var result = await _dbContext.Companies
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Select(c => new CompanyStatsDto
                {
                    CompanyId = c.Id.Value,
                    Name = c.Name,
                    ProblemCount = _dbContext.Set<ProblemCompanyRef>()
                        .Count(r => r.CompanyId == c.Id.Value)
                })
                .OrderByDescending(x => x.ProblemCount)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}