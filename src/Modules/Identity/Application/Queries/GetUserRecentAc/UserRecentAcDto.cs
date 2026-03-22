namespace VAlgo.Modules.Identity.Application.Queries.GetUserRecentAc
{
    public sealed class UserRecentAcDto
    {
        public Guid ProblemId { get; init; }
        public string Code { get; init; } = null!;
        public string Title { get; init; } = null!;
        public Difficulty Difficulty { get; init; }
        public DateTimeOffset LastAcAt { get; init; }

    }

    public enum Difficulty
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
}