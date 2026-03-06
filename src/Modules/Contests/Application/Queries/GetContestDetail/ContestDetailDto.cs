using MediatR;
using VAlgo.Modules.Contests.Domain.Enums;

namespace VAlgo.Modules.Contests.Application.Queries.GetContestDetail
{
    public sealed class ContestDetailDto
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public ContestStatus Status { get; init; }
        public ContestVisibility Visibility { get; init; }
        public int? MaxParticipants { get; init; }
        public Guid CreatedBy { get; init; }
        public DateTime CreatedAt { get; init; }
        public int ParticipantCount { get; init; }
        public List<ContestProblemDto> Problems { get; init; } = new();
    }

    public sealed class ContestProblemDto
    {
        public Guid ProblemId { get; init; }
        public string Code { get; init; } = null!;
        public int Order { get; init; }
        public int Points { get; init; }
    }
}