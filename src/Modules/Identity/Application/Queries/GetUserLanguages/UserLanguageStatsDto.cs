namespace VAlgo.Modules.Identity.Application.Queries.GetUserLanguages
{
    public sealed class UserLanguageStatsDto
    {
        public string Language { get; init; } = null!;
        public int ProblemsSolved { get; init; }
    }
}