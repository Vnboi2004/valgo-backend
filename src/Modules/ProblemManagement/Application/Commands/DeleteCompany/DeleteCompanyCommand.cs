using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.DeleteCompany
{
    public sealed record DeleteCompanyCommand(
        Guid ProblemId,
        Guid CompanyId
    ) : IRequest<Unit>;
}