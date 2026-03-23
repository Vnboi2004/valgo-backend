using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.UpdateCodeTemplate
{
    public sealed record UpdateCodeTemplateCommand(
        Guid ProblemId,
        string Language,
        string UserTemplate,
        string JudgeTemplate

    ) : IRequest<Unit>;
}