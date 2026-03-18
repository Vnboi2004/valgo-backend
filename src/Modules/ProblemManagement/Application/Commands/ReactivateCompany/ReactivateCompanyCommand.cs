using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.ReactivateCompany
{
    public sealed record ReactivateCompanyCommand(Guid CompanyId) : IRequest<Unit>;
}