using MediatR;
using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyList
{
    public sealed record GetCompanyListQuery(
        bool? IsActive,
        int Page,
        int PageSize
    ) : IRequest<PagedResult<CompanyListItemDto>>;
}