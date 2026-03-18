using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyList;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetActiveCompanies
{
    public sealed record GetActiveCompaniesQuery(
        int Page,
        int PageSize
    ) : IRequest<PagedResult<CompanyListItemDto>>;
}