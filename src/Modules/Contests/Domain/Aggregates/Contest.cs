using VAlgo.Modules.Contests.Domain.Entities;
using VAlgo.Modules.Contests.Domain.Enums;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.Aggregates
{
    public sealed class Contest : AggregateRoot<ContestId>
    {
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public string Code { get; private set; } = null!;

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public DateTime? RegistrationStartTime { get; private set; }
        public DateTime? RegistrationEndTime { get; private set; }

        public int? MaxParticipants { get; private set; }

        public bool IsLeaderboardFrozen { get; private set; }
        public DateTime? FreezeAt { get; private set; }

        public ContestStatus Status { get; private set; }
        public ContestVisibility Visibility { get; private set; }
        public ContestType Type { get; private set; }

        public Guid CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public bool AllowPractice { get; private set; }
        public bool AllowVirtual { get; private set; }

        public int TotalSubmissions { get; private set; }
        public TimeSpan Duration => EndTime - StartTime;



        // Navigation properties
        private readonly List<ContestProblem> _problems = new();
        public IReadOnlyCollection<ContestProblem> Problems => _problems;

        private readonly List<ContestParticipant> _participants = new();
        public IReadOnlyCollection<ContestParticipant> Participants => _participants;

        private readonly List<ContestSubmission> _submissions = new();
        public IReadOnlyCollection<ContestSubmission> Submissions => _submissions;

        private Contest() { }

        private Contest(
            ContestId id,
            string title,
            string description,
            string code,
            DateTime startTime,
            DateTime endTime,
            ContestVisibility visibility,
            ContestType type,
            Guid createdBy
        )
            : base(id)
        {
            Title = title;
            Description = description;
            Code = code;
            StartTime = startTime;
            EndTime = endTime;
            Type = type;
            Visibility = visibility;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
            Status = ContestStatus.Draft;
        }

        public static Contest Create(
            string title,
            string description,
            string code,
            DateTime startTime,
            DateTime endTime,
            ContestVisibility visibility,
            ContestType type,
            Guid createdBy
        )
            => new Contest(ContestId.New(), title, description, code, startTime, endTime, visibility, type, createdBy);

        public void AddProblem(Guid problemId, string code, int points)
        {
            // if (Status != ContestStatus.Draft)
            //     throw new InvalidOperationException("Cannot modify problems after published.");

            var order = _problems.Count + 1;
            _problems.Add(ContestProblem.Create(Id, problemId, code, order, points));
        }

        public void Register(Guid userId)
        {
            var now = DateTime.UtcNow;

            if (Status != ContestStatus.Published)
                throw new InvalidOperationException("Contest is not open for registration.");

            if (RegistrationStartTime.HasValue && now < RegistrationStartTime.Value)
                throw new InvalidOperationException("Registration has not started.");

            if (RegistrationEndTime.HasValue && now > RegistrationEndTime.Value)
                throw new InvalidOperationException("Registration has ended.");

            if (MaxParticipants.HasValue && _participants.Count >= MaxParticipants.Value)
                throw new InvalidOperationException("Contest is full.");

            var existing = _participants.FirstOrDefault(x => x.UserId == userId);

            if (existing != null)
            {
                if (existing.IsRegistered)
                    throw new InvalidOperationException("User already registered.");

                existing.MarkRegistered(now);
                return;
            }

            // QUAN TRỌNG: tạo participant mới
            var participant = ContestParticipant.CreateEmpty(Id, userId);
            participant.MarkRegistered(now);

            _participants.Add(participant);
        }

        public void Unregister(Guid userId)
        {
            var now = DateTime.UtcNow;

            // 1. Không cho unregister khi contest đã bắt đầu
            if (Status == ContestStatus.Running || Status == ContestStatus.Finished)
                throw new InvalidOperationException("Cannot unregister after contest started.");

            var participant = _participants.FirstOrDefault(x => x.UserId == userId);

            if (participant == null)
                throw new InvalidOperationException("User is not registered in contest.");

            if (!participant.IsRegistered)
                throw new InvalidOperationException("User is not registered.");

            // 2. Nếu có registration window thì check
            if (RegistrationEndTime.HasValue && now > RegistrationEndTime.Value)
                throw new InvalidOperationException("Registration period has ended.");

            // 
            participant.Unregister();
        }


        public bool AutoStart(DateTime now)
        {
            // 1. Chỉ xử lý khi đang Published
            if (Status != ContestStatus.Published)
                return false;

            // 2. Chưa tới thời gian start thì bỏ qua
            if (now < StartTime)
                return false;

            // 3. Transition trạng thái
            Status = ContestStatus.Running;
            return true;

            // 4. (OPTIONAL) emit domain event
            // AddDomainEvent(new ContestStartedEvent(Id, StartTime));
        }

        public bool AutoFinish(DateTime now)
        {
            // 1. Chỉ xử lý khi đang Running
            if (Status != ContestStatus.Running)
                return false;

            // 2. Chưa tới giờ kết thúc
            if (now < EndTime)
                return false;

            // 3. Freeze leaderboard nếu có config
            if (!IsLeaderboardFrozen && FreezeAt.HasValue && now >= FreezeAt.Value)
            {
                IsLeaderboardFrozen = true;
            }

            // 4. Kết thúc contest
            Status = ContestStatus.Finished;

            return true;
        }

        public bool FreezeLeaderboard(DateTime now)
        {
            if (Status != ContestStatus.Running)
                return false;

            if (!FreezeAt.HasValue || now < FreezeAt.Value)
                return false;

            if (IsLeaderboardFrozen)
                return false;

            IsLeaderboardFrozen = true;

            return true;
        }

        public void Join(Guid userId, string? code = null)
        {
            if (Status != ContestStatus.Running)
                throw new InvalidOperationException("Contest is not running");

            var participant = _participants.FirstOrDefault(x => x.UserId == userId);

            if (participant == null)
                throw new InvalidOperationException("You must register first");

            if (!participant.IsRegistered)
                throw new InvalidOperationException("You must register before joining");

            if (participant.HasJoined)
                throw new InvalidOperationException("Already joined");

            participant.Join(DateTime.UtcNow, _problems.Select(p => p.ProblemId));
        }

        public void ProcessSubmission(Guid userId, Guid problemId, ContestSubmissionVerdict verdict, DateTime submittedAt)
        {
            if (Status != ContestStatus.Running)
                return;

            TotalSubmissions++;

            var participant = _participants.FirstOrDefault(x => x.UserId == userId);
            if (participant == null)
                return;

            var problem = _problems.FirstOrDefault(x => x.ProblemId == problemId);
            if (problem == null)
                return;

            var stat = participant.GetStat(problemId);

            _submissions.Add(ContestSubmission.Create(Id, userId, problemId, verdict, submittedAt));

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

            participant.AddScore(problem.Points);
            participant.AddPenalty(penalty);
            participant.IncrementSubmission();
        }

        public void Publish()
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Contest cannot be published.");

            if (!_problems.Any())
                throw new InvalidOperationException("Contest must have at least one problem.");

            Status = ContestStatus.Published;
        }

        public void Start()
        {
            if (Status != ContestStatus.Published)
                throw new InvalidOperationException("Contest cannot start.");

            Status = ContestStatus.Running;
        }

        public void Finish()
        {
            if (Status != ContestStatus.Running)
                throw new InvalidOperationException("Contest cannot finish.");

            Status = ContestStatus.Finished;
        }

        public void Archive()
        {
            if (Status != ContestStatus.Finished)
                throw new InvalidOperationException("Contest must be finished before archive.");

            Status = ContestStatus.Archived;
        }

        public void RemoveProblem(Guid problemId)
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Cannot modify problems after publish.");

            var problem = _problems.FirstOrDefault(p => p.ProblemId == problemId);
            if (problem == null)
                throw new InvalidOperationException("Problem not found in contest.");

            _problems.Remove(problem);
            ReorderProblems();
        }

        private void ReorderProblems()
        {
            int order = 1;
            foreach (var problem in _problems.OrderBy(x => x.Order))
            {
                problem.UpdateOrder(order);
                order++;
            }
        }

        public void ReorderProblems(IReadOnlyList<Guid> problemIds)
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Cannot reorder problems after contest is published.");

            if (problemIds.Count != _problems.Count)
                throw new InvalidOperationException("Problem list mismatch.");

            var problemMap = _problems.ToDictionary(x => x.ProblemId);

            int order = 1;

            foreach (var problemId in problemIds)
            {
                if (!problemMap.TryGetValue(problemId, out var problem))
                    throw new InvalidOperationException("Problem does not belong to this contest.");

                problem.UpdateOrder(order);
                order++;
            }
        }

        public void UpdateProblemPoints(Guid problemId, int points)
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Cannot modify problems after contest is published.");

            if (points <= 0)
                throw new InvalidOperationException("Points must be greater than zero.");

            var problem = _problems.FirstOrDefault(x => x.ProblemId == problemId);
            if (problem == null)
                throw new InvalidOperationException("Problem not found in contest.");

            problem.UpdatePoints(points);
        }

        public void UpdateMetadata(string title, string description)
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Cannot update contest after publish.");

            Title = title;
            Description = description;
        }

        public void UpdateSchedule(DateTime startTime, DateTime endTime)
        {
            // if (Status != ContestStatus.Draft)
            //     throw new InvalidOperationException("Cannot update schedule after publish.");

            if (startTime >= endTime)
                throw new InvalidOperationException("Invalid contest time.");

            StartTime = startTime;
            EndTime = endTime;
        }

        public void UpdateVisibility(ContestVisibility visibility)
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Cannot change visibility after publish.");

            Visibility = visibility;
        }

        public void UpdateMaxParticipants(int? maxParticipants)
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Cannot update max partipants after publish.");

            MaxParticipants = maxParticipants;
        }

        public void Leave(Guid userId)
        {
            if (Status == ContestStatus.Running)
                throw new InvalidOperationException("Cannot leave during contest.");

            var participant = _participants.FirstOrDefault(x => x.UserId == userId);

            if (participant == null)
                throw new InvalidOperationException("User is not a participant of the contest.");

            _participants.Remove(participant);
        }

        public ContestParticipant? GetParticipant(Guid userId)
        {
            return _participants.FirstOrDefault(p => p.UserId == userId);
        }

        public void ResetLeaderboard()
        {
            foreach (var participant in _participants)
            {
                participant.ResetStats();
            }

            _submissions.Clear();
            TotalSubmissions = 0;
        }

        public void RemoveParticipant(Guid userId)
        {
            var participant = _participants.FirstOrDefault(x => x.UserId == userId);

            if (participant == null)
                throw new InvalidOperationException("Participant not found.");

            _participants.Remove(participant);
        }
    }
}