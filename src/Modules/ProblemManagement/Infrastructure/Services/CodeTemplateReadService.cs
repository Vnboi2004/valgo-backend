using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Services
{
    public sealed class CodeTemplateReadService : ICodeTemplateReadService
    {
        private readonly ICodeTemplateReadRepository _codeTemplateReadRepository;

        public CodeTemplateReadService(ICodeTemplateReadRepository codeTemplateReadRepository)
        {
            _codeTemplateReadRepository = codeTemplateReadRepository;
        }

        public async Task<CodeTemplateForJudgeDto?> GetTemplateAsync(Guid problemId, string language, CancellationToken cancellationToken = default)
        {
            var data = await _codeTemplateReadRepository.GetJudgeTemplateAsync(problemId, language, cancellationToken);

            if (data == null)
                return null;

            return new CodeTemplateForJudgeDto
            {
                UserTemplate = data.UserTemplate,
                JudgeTemplateHeader = data.JudgeTemplateHeader,
                JudgeTemplateFooter = data.JudgeTemplateFooter
            };
        }
    }
}