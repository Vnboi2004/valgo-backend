using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplate
{
    public sealed record GetCodeTemplateQuery(Guid ProblemId, string Language) : IRequest<CodeTemplateDto>;
}