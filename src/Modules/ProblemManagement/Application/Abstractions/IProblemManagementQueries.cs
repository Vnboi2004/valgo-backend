using VAlgo.Modules.ProblemManagement.Application.Queries.GetClassificationStats;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemCompanies;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemTags;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetSimilarProblems;
using VAlgo.SharedKernel.CrossModule.Classifications;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface IProblemManagementQueries
    {
        Task<(IReadOnlyList<ProblemListItemDto> Items, int TotalCount)> GetListAsync(
            GetProblemListQuery filter,
            int skip,
            int take,
            CancellationToken cancellationToken = default
        );
        Task<ProblemDetailDto?> GetDetailAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<ProblemEditorDto?> GetEditorAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SimilarProblemDto>> GetSimilarProblemsAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProblemCompanyDto>> GetProblemCompaniesAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProblemTagDto>> GetProblemTagsAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ClassificationStatsDto>> GetClassificationStatsAsync(ClassificationType? type, CancellationToken cancellationToken = default);
    }
}