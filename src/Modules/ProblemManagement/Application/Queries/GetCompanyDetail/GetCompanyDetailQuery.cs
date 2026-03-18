using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCompanyDetail
{
    public sealed record GetCompanyDetailQuery(Guid CompanyId) : IRequest<CompanyDetailDto>;
}