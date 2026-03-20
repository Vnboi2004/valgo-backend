

using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.ProblemManagement.Application.Abstractions
{
    public interface ISubmissionReadService
    {
        Task<UserProblemStatus> GetUserProblemStatusAsync(Guid userId, Guid problemId, CancellationToken cancellationToken);

        Task<Dictionary<Guid, UserProblemStatus>> GetUserProblemStatusBatchAsync(Guid userId, IReadOnlyList<Guid> problemIds, CancellationToken cancellationToken);
    }
}