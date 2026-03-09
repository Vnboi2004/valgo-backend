using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.Entities
{
    public sealed class ContestParticipant : Entity<ContestParticipantId>
    {
        public ContestId ContestId { get; private set; } = null!;
        public Guid UserId { get; private set; }
        public DateTime JoinedAt { get; private set; }
        public int Score { get; private set; }
        public int Penalty { get; private set; }
        public int Rank { get; private set; }
        public DateTime? LastSubmissionAt { get; private set; }


        private ContestParticipant() { }

        private ContestParticipant(ContestParticipantId id, ContestId contestId, Guid userId, DateTime joinedAt)
            : base(id)
        {
            ContestId = contestId;
            UserId = userId;
            JoinedAt = joinedAt;
            Score = 0;
            Penalty = 0;
        }

        public static ContestParticipant Create(ContestId contestId, Guid userId, DateTime joinedAt)
            => new ContestParticipant(ContestParticipantId.New(), contestId, userId, joinedAt);

        public void AddScore(int score)
        {
            Score += score;
        }

        public void AddPenalty(int penalty)
        {
            Penalty += penalty;
        }
    }
}