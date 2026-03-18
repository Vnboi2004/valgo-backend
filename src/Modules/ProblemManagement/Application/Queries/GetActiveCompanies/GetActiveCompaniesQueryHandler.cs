using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyList;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetActiveCompanies
{
    public sealed class GetActiveCompaniesQueryHandler : IRequestHandler<GetActiveCompaniesQuery, PagedResult<CompanyListItemDto>>
    {
        private readonly ICompanyReadRepository _readRepository;

        public GetActiveCompaniesQueryHandler(ICompanyReadRepository readRepository)
        {
            _readRepository = readRepository;
        }

        public async Task<PagedResult<CompanyListItemDto>> Handle(GetActiveCompaniesQuery request, CancellationToken cancellationToken)
        {
            return await _readRepository.GetListAsync(isActive: true, request.Page, request.PageSize, cancellationToken);
        }
    }
}