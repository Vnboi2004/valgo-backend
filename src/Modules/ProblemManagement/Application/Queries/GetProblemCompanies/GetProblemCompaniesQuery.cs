using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemCompanies
{
    public sealed record GetProblemCompaniesQuery(Guid ProblemId) : IRequest<IReadOnlyList<ProblemCompanyDto>>;
}