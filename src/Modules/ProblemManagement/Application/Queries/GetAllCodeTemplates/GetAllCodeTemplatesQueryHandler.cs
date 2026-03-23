using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetAllCodeTemplates
{
    public sealed class GetAllCodeTemplatesQueryHandler : IRequestHandler<GetAllCodeTemplatesQuery, IReadOnlyList<CodeTemplateEditorDto>>
    {
        private readonly ICodeTemplateReadRepository _codeTemplateReadRepository;

        public GetAllCodeTemplatesQueryHandler(ICodeTemplateReadRepository codeTemplateReadRepository)
        {
            _codeTemplateReadRepository = codeTemplateReadRepository;
        }

        public async Task<IReadOnlyList<CodeTemplateEditorDto>> Handle(GetAllCodeTemplatesQuery request, CancellationToken cancellationToken)
        {
            return await _codeTemplateReadRepository.GetAllTemplatesAsync(request.ProblemId, cancellationToken);
        }
    }
}