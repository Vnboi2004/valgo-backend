using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.DeactivateCompany
{
    public sealed record DeactivateCompanyCommand(Guid CompanyId) : IRequest<Unit>;                                             
}