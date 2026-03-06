using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.Modules.Contests.Application.Queries.GetContests
{
    public sealed class ContestListItemDto
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = null!;

        public DateTime StartTime { get; init; }

        public DateTime EndTime { get; init; }

        public ContestStatus Status { get; init; }
        public ContestVisibility Visibility { get; init; }

        public int ParticipantCount { get; init; }
    }
}