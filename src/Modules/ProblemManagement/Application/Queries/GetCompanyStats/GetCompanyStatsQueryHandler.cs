using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyStats
{
    public sealed class GetCompanyStatsQueryHandler : IRequestHandler<GetCompanyStatsQuery, IReadOnlyList<CompanyStatsDto>>
    {
        private readonly ICompanyReadRepository _companyReadRepository;

        public GetCompanyStatsQueryHandler(ICompanyReadRepository companyReadRepository)
        {
            _companyReadRepository = companyReadRepository;
        }

        public async Task<IReadOnlyList<CompanyStatsDto>> Handle(GetCompanyStatsQuery request, CancellationToken cancellationToken)
        {
            return await _companyReadRepository.GetCompanyStatsAsync(cancellationToken);
        }
    }
}