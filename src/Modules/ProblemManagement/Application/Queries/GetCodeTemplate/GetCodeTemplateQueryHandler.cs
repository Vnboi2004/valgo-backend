using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplate
{
    public sealed class GetCodeTemplateQueryHandler : IRequestHandler<GetCodeTemplateQuery, CodeTemplateDto>
    {
        private readonly ICodeTemplateReadRepository _codeTemplateReadRepository;

        public GetCodeTemplateQueryHandler(ICodeTemplateReadRepository codeTemplateReadRepository)
        {
            _codeTemplateReadRepository = codeTemplateReadRepository;
        }

        public async Task<CodeTemplateDto> Handle(GetCodeTemplateQuery request, CancellationToken cancellationToken)
        {
            return await _codeTemplateReadRepository.GetUserTemplateAsync(request.ProblemId, request.Language, cancellationToken)
                ?? throw new CodeTemplateNotFoundException(request.Language);
        }
    }
}