using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyStats
{
    public sealed record GetCompanyStatsQuery() : IRequest<IReadOnlyList<CompanyStatsDto>>;
}