using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Commands.AddCodeTemplate
{
    public sealed record AddCodeTemplateCommand(
        Guid ProblemId,
        string Language,
        string UserTemplate,
        string JudgeTemplateHeader,
        string JudgeTemplateFooter
    ) : IRequest<Unit>;
}