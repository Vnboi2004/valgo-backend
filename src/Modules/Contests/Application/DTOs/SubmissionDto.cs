using VAlgo.SharedKernel.CrossModule.Submissions;

namespace VAlgo.Modules.Contests.Application.DTOs
{
    public sealed class SubmissionDto
    {
        public Guid UserId { get; init; }
        public Guid ProblemId { get; init; }
        public Verdict Verdict { get; init; }
        public DateTime SubmittedAt { get; init; }
    }
}