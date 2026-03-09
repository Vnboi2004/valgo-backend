namespace VAlgo.Modules.Contests.Application.Queries.GetContestLeaderboard
{
    public sealed class ContestLeaderboardItemDto
    {
        public int Rank { get; init; }

        public Guid UserId { get; init; }

        public int Score { get; init; }

        public int Penalty { get; init; }

        public int SolvedProblems { get; init; }
    }
}