using MediatR;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Domain.Exceptions;

namespace VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplateForJudge
{
    public sealed class GetCodeTemplateForJudgeQueryHandler : IRequestHandler<GetCodeTemplateForJudgeQuery, CodeTemplateForJudgeDto>
    {
        private readonly ICodeTemplateReadRepository _codeTemplateReadRepository;

        public GetCodeTemplateForJudgeQueryHandler(ICodeTemplateReadRepository codeTemplateReadRepository)
        {
            _codeTemplateReadRepository = codeTemplateReadRepository;
        }

        public async Task<CodeTemplateForJudgeDto> Handle(GetCodeTemplateForJudgeQuery request, CancellationToken cancellationToken)
        {
            return await _codeTemplateReadRepository.GetJudgeTemplateAsync(request.ProblemId, request.Language, cancellationToken)
                ?? throw new CodeTemplateNotFoundException(request.Language);
        }
    }
}