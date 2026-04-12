using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.Entities
{
    public sealed class ContestSubmission : Entity<ContestSubmissionId>
    {
        public ContestId ContestId { get; private set; } = null!;
        public Guid UserId { get; private set; }
        public Guid ProblemId { get; private set; }
        public ContestSubmissionVerdict Verdict { get; private set; }
        public DateTime SubmittedAt { get; private set; }

        private ContestSubmission() { }

        private ContestSubmission(ContestSubmissionId id, ContestId contestId, Guid userId, Guid problemId, ContestSubmissionVerdict verdict, DateTime submittedAt)
            : base(id)
        {
            ContestId = contestId;
            UserId = userId;
            ProblemId = problemId;
            Verdict = verdict;
            SubmittedAt = submittedAt;
        }

        public static ContestSubmission Create(ContestId contestId, Guid userId, Guid problemId, ContestSubmissionVerdict verdict, DateTime submittedAt)
            => new ContestSubmission(ContestSubmissionId.New(), contestId, userId, problemId, verdict, submittedAt);
    }
}