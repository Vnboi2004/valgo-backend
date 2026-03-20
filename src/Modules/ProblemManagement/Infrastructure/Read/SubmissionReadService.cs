using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Application.Queries.GetUserProblemStatus;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;
using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Read
{
    public sealed class SubmissionReadService : ISubmissionReadService
    {
        private readonly SubmissionsDbContext _submissionDbContext;

        public SubmissionReadService(SubmissionsDbContext submissionDbContext)
        {
            _submissionDbContext = submissionDbContext;
        }

        public async Task<UserProblemStatus> GetUserProblemStatusAsync(
            Guid userId,
            Guid problemId,
            CancellationToken cancellationToken)
        {
            var hasSolved = await _submissionDbContext.Submissions
                .AsNoTracking()
                .AnyAsync(s =>
                    s.UserId == userId &&
                    s.ProblemId == problemId &&
                    s.Verdict == Verdict.Accepted,
                    cancellationToken);

            if (hasSolved) return UserProblemStatus.Solved;

            var hasAttempted = await _submissionDbContext.Submissions
                .AsNoTracking()
                .AnyAsync(s =>
                    s.UserId == userId &&
                    s.ProblemId == problemId,
                    cancellationToken);

            return hasAttempted
                ? UserProblemStatus.Attempted
                : UserProblemStatus.NotAttempted;
        }

        public async Task<Dictionary<Guid, UserProblemStatus>> GetUserProblemStatusBatchAsync(
            Guid userId,
            IReadOnlyList<Guid> problemIds,
            CancellationToken cancellationToken)
        {
            var submissions = await _submissionDbContext.Submissions
                .AsNoTracking()
                .Where(s =>
                    s.UserId == userId &&
                    problemIds.Contains(s.ProblemId) &&
                    s.Status == SubmissionStatus.Completed)
                .Select(s => new { s.ProblemId, s.Verdict })
                .ToListAsync(cancellationToken);

            return submissions
                .GroupBy(s => s.ProblemId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Any(s => s.Verdict == Verdict.Accepted)
                        ? UserProblemStatus.Solved
                        : UserProblemStatus.Attempted
                );
        }
    }
}