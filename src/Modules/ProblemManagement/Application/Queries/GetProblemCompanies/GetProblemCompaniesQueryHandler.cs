using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemCompanies
{
    public sealed class GetProblemCompaniesQueryHandler : IRequestHandler<GetProblemCompaniesQuery, IReadOnlyList<ProblemCompanyDto>>
    {
        private readonly IProblemManagementQueries _problemManagementQueries;

        public GetProblemCompaniesQueryHandler(IProblemManagementQueries problemManagementQueries)
        {
            _problemManagementQueries = problemManagementQueries;
        }

        public async Task<IReadOnlyList<ProblemCompanyDto>> Handle(
            GetProblemCompaniesQuery request,
            CancellationToken cancellationToken)
        {
            return await _problemManagementQueries.GetProblemCompaniesAsync(request.ProblemId, cancellationToken);
        }
    }
}