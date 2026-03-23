using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetAllCodeTemplates;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplate;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetCodeTemplateForJudge;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Read
{
    public sealed class CodeTemplateReadRepository : ICodeTemplateReadRepository
    {
        private readonly ProblemManagementDbContext _dbContext;

        public CodeTemplateReadRepository(ProblemManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CodeTemplateDto?> GetUserTemplateAsync(Guid problemId, string language, CancellationToken cancellationToken)
        {
            var normalizedLanguage = language.Trim().ToLowerInvariant();

            return await _dbContext.Problems
                .AsNoTracking()
                .Where(p =>
                    p.Id == ProblemId.From(problemId) &&
                    p.Status == ProblemStatus.Published)
                .SelectMany(p => p.CodeTemplates)
                .Where(t => t.Language == normalizedLanguage)
                .Select(t => new CodeTemplateDto
                {
                    Language = t.Language,
                    UserTemplate = t.UserTemplate
                    // JudgeTemplate không select — bảo mật
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<CodeTemplateForJudgeDto?> GetJudgeTemplateAsync(Guid problemId, string language, CancellationToken cancellationToken)
        {
            var normalizedLanguage = language.Trim().ToLowerInvariant();

            return await _dbContext.Problems
                .AsNoTracking()
                .Where(p => p.Id == ProblemId.From(problemId))
                .SelectMany(p => p.CodeTemplates)
                .Where(t => t.Language == normalizedLanguage)
                .Select(t => new CodeTemplateForJudgeDto
                {
                    Language = t.Language,
                    UserTemplate = t.UserTemplate,
                    JudgeTemplate = t.JudgeTemplate
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<CodeTemplateEditorDto>> GetAllTemplatesAsync(
            Guid problemId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Problems
                .AsNoTracking()
                .Where(p => p.Id == ProblemId.From(problemId))
                .SelectMany(p => p.CodeTemplates)
                .Select(t => new CodeTemplateEditorDto
                {
                    Language = t.Language,
                    UserTemplate = t.UserTemplate,
                    JudgeTemplate = t.JudgeTemplate
                })
                .ToListAsync(cancellationToken);
        }
    }
}