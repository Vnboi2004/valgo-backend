using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.DeleteCodeTemplate
{
    public sealed record DeleteCodeTemplateCommand(Guid ProblemId, string Language) : IRequest<Unit>;
}