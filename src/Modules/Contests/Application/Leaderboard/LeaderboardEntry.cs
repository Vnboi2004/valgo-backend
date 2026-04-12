namespace VAlgo.Modules.Contests.Application.Leaderboard
{
    public sealed class LeaderboardEntry
    {
        public Guid UserId { get; }
        public int Score { get; }
        public int Penalty { get; }
        public int SolvedProblems { get; }

        public LeaderboardEntry(Guid userId, int score, int penalty, int solvedProblems)
        {
            UserId = userId;
            Score = score;
            Penalty = penalty;
            SolvedProblems = solvedProblems;
        }
    }
}