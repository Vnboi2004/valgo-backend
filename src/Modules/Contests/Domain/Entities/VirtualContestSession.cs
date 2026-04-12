using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.Entities
{
    public sealed class VirtualContestSession : Entity<VirtualContestSessionId>
    {
        public ContestId ContestId { get; private set; } = null!;
        public Guid UserId { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public int Score { get; private set; }
        public int Penalty { get; private set; }
        public readonly List<ParticipantProblemStat> _problemStats = new();
        public IReadOnlyCollection<ParticipantProblemStat> ProblemStats => _problemStats;

        private VirtualContestSession() { }

        private VirtualContestSession(
            VirtualContestSessionId id,
            ContestId contestId,
            Guid userId,
            DateTime startTime,
            TimeSpan duration,
            IEnumerable<Guid> problemIds
        )
            : base(id)
        {
            ContestId = contestId;
            UserId = userId;
            StartTime = startTime;
            EndTime = startTime.Add(duration);

            foreach (var problemId in problemIds)
            {
                _problemStats.Add(ParticipantProblemStat.CreateForVirtual(problemId, null!));
            }
        }

        public static VirtualContestSession Create(
            ContestId contestId,
            Guid userId,
            DateTime startTime,
            TimeSpan now,
            IEnumerable<Guid> problemIds
        )
        {
            return new VirtualContestSession(VirtualContestSessionId.New(), contestId, userId, startTime, now, problemIds);
        }

        public void ProcessSubmission(Guid problemId, ContestSubmissionVerdict verdict, DateTime submittedAt, int problemPoints)
        {
            if (submittedAt > EndTime)
                return;

            var stat = _problemStats.First(x => x.ProblemId == problemId);

            if (stat.IsSolved)
                return;

            if (verdict != ContestSubmissionVerdict.Accepted)
            {
                stat.AddWrong();
                return;
            }

            stat.MarkSolved(submittedAt);

            var minutes = (int)(submittedAt - StartTime).TotalMinutes;
            var penalty = minutes + stat.WrongAttempts * 20;

            Score += problemPoints;
            Penalty += penalty;
        }
    }
}