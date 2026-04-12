using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.Entities
{
    public sealed class ParticipantProblemStat : Entity<ParticipantProblemStatId>
    {
        public ContestParticipantId? ContestParticipantId { get; private set; }
        public VirtualContestSessionId? VirtualContestSessionId { get; private set; }
        public Guid ProblemId { get; private set; }
        public bool IsSolved { get; private set; }
        public int WrongAttempts { get; private set; }
        public DateTime? SolvedAt { get; private set; }

        private ParticipantProblemStat() { }

        private ParticipantProblemStat(ParticipantProblemStatId id, Guid problemId, ContestParticipantId? contestParticipantId, VirtualContestSessionId? virtualContestSessionId)
            : base(id)
        {
            ProblemId = problemId;
            ContestParticipantId = contestParticipantId;
            VirtualContestSessionId = virtualContestSessionId;

            if ((contestParticipantId == null && virtualContestSessionId == null) ||
                (contestParticipantId != null && virtualContestSessionId != null))
            {
                throw new InvalidOperationException("Stat must belong to either Contest or VirtualContest.");
            }
        }

        public static ParticipantProblemStat CreateForContest(Guid problemId, ContestParticipantId contestParticipantId)
            => new ParticipantProblemStat(ParticipantProblemStatId.New(), problemId, contestParticipantId, null);

        public static ParticipantProblemStat CreateForVirtual(Guid problemId, VirtualContestSessionId virtualContestSessionId)
            => new ParticipantProblemStat(ParticipantProblemStatId.New(), problemId, null, virtualContestSessionId);

        public void AddWrong()
        {
            if (IsSolved) return;
            WrongAttempts++;
        }

        public void MarkSolved(DateTime time)
        {
            if (IsSolved) return;
            IsSolved = true;
            SolvedAt = time;
        }

        public void Reset()
        {
            IsSolved = false;
            WrongAttempts = 0;
            SolvedAt = null;
        }
    }
}