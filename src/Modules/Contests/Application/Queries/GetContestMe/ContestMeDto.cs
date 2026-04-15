namespace VAlgo.Modules.Contests.Application.Queries.GetContestMe
{
    public sealed class ContestMeDto
    {
        public bool IsRegistered { get; init; }
        public bool HasJoined { get; init; }
        public DateTime? RegisteredAt { get; init; }
        public DateTime? JoinedAt { get; init; }
        public bool CanRegister { get; init; }
        public bool Canjoin { get; init; }
        public bool CanStartVirtual { get; init; }
    }
}