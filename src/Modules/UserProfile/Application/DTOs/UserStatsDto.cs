namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed class UserStatsDto
    {
        public int TotalSolved { get; init; }
        public int EasySolved { get; init; }
        public int MediumSolved { get; init; }
        public int HardSolved { get; init; }
        public int EasyTotal { get; init; }
        public int MediumTotal { get; init; }
        public int HardTotal { get; init; }
        public int TotalSubmissions { get; init; }
        public int AcceptedSubmissions { get; init; }
        public double AcceptanceRate => TotalSubmissions == 0
            ? 0
            : Math.Round((double)AcceptedSubmissions / TotalSubmissions * 100, 1);
    }
}