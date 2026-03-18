using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Repositories
{
    public sealed class CompanyRepository : ICompanyRepository
    {
        private readonly ProblemManagementDbContext _dbContext;

        public CompanyRepository(ProblemManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
        {
            await _dbContext.Companies.AddAsync(company, cancellationToken);
        }

        public async Task<Company?> GetByIdAsync(CompanyId companyId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id.Value == companyId.Value, cancellationToken);
        }
    }
}