using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyDetail
{
    public sealed class GetCompanyDetailQueryHandler : IRequestHandler<GetCompanyDetailQuery, CompanyDetailDto>
    {
        private readonly ICompanyReadRepository _companyReadRepository;

        public GetCompanyDetailQueryHandler(ICompanyReadRepository companyReadRepository)
        {
            _companyReadRepository = companyReadRepository;
        }

        public async Task<CompanyDetailDto> Handle(GetCompanyDetailQuery request, CancellationToken cancellationToken)
        {
            var company = await _companyReadRepository.GetDetailAsync(request.CompanyId, cancellationToken);

            if (company == null)
                throw new InvalidOperationException("Company not found.");

            return company;
        }
    }
}