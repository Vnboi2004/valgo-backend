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
        public int SubmissionCount { get; private set; }
        public bool IsRegistered { get; private set; }
        public bool HasJoined { get; private set; }
        public DateTime? RegisteredAt { get; private set; }
        private readonly List<ParticipantProblemStat> _problemStats = new();
        public IReadOnlyCollection<ParticipantProblemStat> ProblemStats => _problemStats;
        public int SolvedCount => _problemStats.Count(x => x.IsSolved);


        private ContestParticipant() { }

        private ContestParticipant(ContestParticipantId id, ContestId contestId, Guid userId, DateTime joinedAt, IEnumerable<Guid> problemIds)
            : base(id)
        {
            ContestId = contestId;
            UserId = userId;
            JoinedAt = joinedAt;
            Score = 0;
            Penalty = 0;

            foreach (var problemId in problemIds)
            {
                _problemStats.Add(ParticipantProblemStat.CreateForContest(problemId, Id));
            }
        }

        public static ContestParticipant Create(ContestId contestId, Guid userId, DateTime joinedAt, IEnumerable<Guid> problemIds)
            => new ContestParticipant(ContestParticipantId.New(), contestId, userId, joinedAt, problemIds);


        public static ContestParticipant CreateEmpty(ContestId contestId, Guid userId)
        {
            return new ContestParticipant
            {
                Id = ContestParticipantId.New(),
                ContestId = contestId,
                UserId = userId,
                Score = 0,
                Penalty = 0,
                SubmissionCount = 0,
                IsRegistered = false,
                HasJoined = false
            };
        }

        public ParticipantProblemStat GetStat(Guid problemId)
        {
            return _problemStats.First(x => x.ProblemId == problemId);
        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public void AddPenalty(int penalty)
        {
            Penalty += penalty;
        }

        public void IncrementSubmission()
        {
            SubmissionCount++;
        }

        public void MarkRegistered(DateTime time)
        {
            if (IsRegistered)
                return;

            IsRegistered = true;
            RegisteredAt = time;
        }

        public void Unregister()
        {
            if (!IsRegistered)
                return;

            IsRegistered = false;
            RegisteredAt = null;
        }

        public void Join(DateTime time, IEnumerable<Guid> problemIds)
        {
            if (!IsRegistered)
                throw new InvalidOperationException("Must register first");

            if (HasJoined)
                throw new InvalidOperationException("Already joined");

            HasJoined = true;
            JoinedAt = time;

            _problemStats.Clear();

            foreach (var problemId in problemIds)
            {
                _problemStats.Add(ParticipantProblemStat.CreateForContest(problemId, Id));
            }
        }

        public void ResetStats()
        {
            Score = 0;
            Penalty = 0;

            foreach (var stat in _problemStats)
            {
                stat.Reset();
            }
        }
    }
}