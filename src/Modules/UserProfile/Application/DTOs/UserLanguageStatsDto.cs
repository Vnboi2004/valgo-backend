namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed class UserLanguageStatsDto
    {
        public string Language { get; init; } = null!;
        public int ProblemsSolved { get; init; }
    }
}