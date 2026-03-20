using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyList;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyStats;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface ICompanyReadRepository
    {
        Task<PagedResult<CompanyListItemDto>> GetListAsync(bool? isActive, int page, int pageSize, CancellationToken cancellationToken = default);
        Task<CompanyDetailDto?> GetDetailAsync(Guid companyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CompanyStatsDto>> GetCompanyStatsAsync(CancellationToken cancellationToken = default);
    }
}