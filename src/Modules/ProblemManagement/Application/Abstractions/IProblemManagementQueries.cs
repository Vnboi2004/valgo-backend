using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;

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
    }
}