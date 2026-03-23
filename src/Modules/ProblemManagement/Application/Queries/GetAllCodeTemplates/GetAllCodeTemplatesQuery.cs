using MediatR;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetAllCodeTemplates
{
    public sealed record GetAllCodeTemplatesQuery(Guid ProblemId) : IRequest<IReadOnlyList<CodeTemplateEditorDto>>;
}