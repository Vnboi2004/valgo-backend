using VAlgo.Modules.ProblemManagement.Application.Queries.GetAllCodeTemplates;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplate;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplateForJudge;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface ICodeTemplateReadRepository
    {
        Task<CodeTemplateDto?> GetUserTemplateAsync(Guid problemId, string language, CancellationToken cancellationToken);

        Task<CodeTemplateForJudgeDto?> GetJudgeTemplateAsync(Guid problemId, string language, CancellationToken cancellationToken);

        Task<IReadOnlyList<CodeTemplateEditorDto>> GetAllTemplatesAsync(Guid problemId, CancellationToken cancellationToken);
    }
}