using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.CreateCompany
{
    public sealed record CreateCompanyCommand(string Name) : IRequest<Guid>;                                                                        
}