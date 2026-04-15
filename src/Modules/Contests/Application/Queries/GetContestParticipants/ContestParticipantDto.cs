namespace VAlgo.Modules.Contests.Application.Queries.GetContestParticipants
{
    public sealed class ContestParticipantDto
    {
        public int TotalCount { get; init; }
        public IReadOnlyList<ContestParticipantItemDto> Participants { get; init; } = [];
    }

    public sealed class ContestParticipantItemDto
    {
        public Guid UserId { get; init; }
        public DateTime JoinedAt { get; init; }
    }
}