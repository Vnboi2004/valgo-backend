using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.RenameCompany
{
    public sealed record RenameCompanyCommand(
        Guid CompanyId,
        string Name
    ) : IRequest<Unit>;
}