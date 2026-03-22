using VAlgo.Modules.ProblemManagement.Application.Queries.GetClassificationStats;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemCompanies;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditorial;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemHints;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemListWithUserStatus;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemTags;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetRandomProblem;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetSimilarProblems;
using VAlgo.Modules.ProblemManagement.Domain.Enums;
using VAlgo.SharedKernel.CrossModule.Classifications;
using VAlgo.SharedKernel.CrossModule.Problems;
using VAlgo.SharedKernel.Domain;

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
        Task<PagedResult<ProblemListRawDto>> GetProblemListAsync(
            string? keyword,
            Difficulty? difficulty,
            ProblemStatus? status,
            Guid? companyId,
            Guid? classificationId,
            ProblemSortBy sortBy,
            SortDirection sortDirection,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task<ProblemEditorialDto?> GetEditorialAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<bool> ProblemExistsAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProblemHintDto>> GetHintsAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<RandomProblemDto?> GetRandomAsync(Difficulty? difficulty, Guid? classificationId, CancellationToken cancellationToken = default);
    }
}