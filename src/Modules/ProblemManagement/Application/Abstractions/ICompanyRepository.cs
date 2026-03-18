using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface ICompanyRepository
    {
        Task AddAsync(Company company, CancellationToken cancellationToken = default);                      
        Task<Company?> GetByIdAsync(CompanyId companyId, CancellationToken cancellationToken = default);
    }
}