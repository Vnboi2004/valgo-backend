namespace VAlgo.Modules.Contests.Application.Queries.GetContestParticipants
{
    public sealed class ContestParticipantDto
    {
        public Guid UserId { get; init; }
        public DateTime JoinedAt { get; init; }
        public int Score { get; init; }
        public int Penalty { get; init; }
    }
}