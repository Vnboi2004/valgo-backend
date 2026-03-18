using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyList
{
    public sealed class GetCompanyListQueryHandler : IRequestHandler<GetCompanyListQuery, PagedResult<CompanyListItemDto>>
    {
        private readonly ICompanyReadRepository _companyReadRepository;

        public GetCompanyListQueryHandler(ICompanyReadRepository companyReadRepository)
        {
            _companyReadRepository = companyReadRepository;
        }

        public async Task<PagedResult<CompanyListItemDto>> Handle(GetCompanyListQuery request, CancellationToken cancellationToken)
        {
            return await _companyReadRepository.GetListAsync(request.IsActive, request.Page, request.PageSize, cancellationToken);
        }
    }
}