using VAlgo.SharedKernel.CrossModule.Problems;

namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed class UserRecentAcDto
    {
        public Guid ProblemId { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public Difficulty Difficulty { get; init; }
        public DateTimeOffset LastAcAt { get; init; }
    }
}