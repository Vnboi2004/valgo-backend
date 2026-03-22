namespace VAlgo.Modules.Identity.Application.Queries.GetUserPublicProfile
{
    public sealed class UserPublicProfileDto
    {
        public Guid UserId { get; init; }
        public string Username { get; init; } = null!;
        public string? DisplayName { get; init; }
        public string? Avatar { get; init; }
        public string? Location { get; init; }
        public string? Website { get; init; }
        public string? Github { get; init; }
        public string? LinkedIn { get; init; }
        public string? Twitter { get; init; }
        public string? ReadMe { get; init; }
        public string? Work { get; init; }
        public string? Education { get; init; }
        public bool ShowRecentSubmissions { get; init; }
        public bool ShowSubmissionHeatmap { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public int Following { get; init; } = 0;
        public int Followers { get; init; } = 0;
        public int Rank { get; init; } = 0;
    }
}