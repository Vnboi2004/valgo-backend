using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AddCompany
{
    public sealed record AddCompanyCommand(
        Guid ProblemId,
        Guid CompanyId
    ) : IRequest<Unit>;
}