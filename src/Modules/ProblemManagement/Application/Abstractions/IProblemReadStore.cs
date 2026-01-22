using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemDetail;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemEditor;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetProblemList;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface IProblemReadStore
    {
        Task<(IReadOnlyList<ProblemListItemDto> Items, int TotalCount)> GetProblemsAsync(
            GetProblemListQuery filter,
            int skip,
            int take,
            CancellationToken cancellationToken = default
        );
        Task<ProblemDetailDto?> GetProblemDetailAsync(Guid problemId, CancellationToken cancellationToken = default);
        Task<ProblemEditorDto?> GetAsync(Guid problemId, CancellationToken cancellationToken = default);
    }
}