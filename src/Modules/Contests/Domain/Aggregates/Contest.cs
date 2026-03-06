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
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public ContestStatus Status { get; private set; }
        public ContestVisibility Visibility { get; private set; }
        public int? MaxParticipants { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navigation properties
        private readonly List<ContestProblem> _problems = new();
        public IReadOnlyCollection<ContestProblem> Problems => _problems;

        private readonly List<ContestParticipant> _participants = new();
        public IReadOnlyCollection<ContestParticipant> Participants => _participants;

        private Contest() { }

        private Contest(ContestId id, string title, string description, DateTime startTime, DateTime endTime, Guid createdBy)
            : base(id)
        {
            Title = title;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            Status = ContestStatus.Draft;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public static Contest Create(string title, string description, DateTime startTime, DateTime endTime, Guid createdBy)
            => new Contest(ContestId.New(), title, description, startTime, endTime, createdBy);

        public void AddProblem(Guid problemId, string code, int points)
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Cannot modify problems after published.");

            if (_problems.Any(x => x.ProblemId == problemId))
                throw new InvalidOperationException("Problem already exists in the contest.");

            if (_problems.Any(x => x.Code == code))
                throw new InvalidOperationException("Problem code already exists.");

            var order = _problems.Count + 1;

            var problem = ContestProblem.Create(Id, problemId, code, order, points);
            _problems.Add(problem);
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

        public void Join(Guid userId)
        {
            if (Status != ContestStatus.Published && Status != ContestStatus.Running)
                throw new InvalidOperationException("Contest is not open for joining.");

            if (MaxParticipants.HasValue && _participants.Count >= MaxParticipants.Value)
                throw new InvalidOperationException("Contest has reached maximum participants.");

            if (_participants.Any(x => x.UserId == userId))
                throw new InvalidOperationException("User already joined the contest.");

            var participant = ContestParticipant.Create(Id, userId, DateTime.Now);

            _participants.Add(participant);
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

        public void Publish()
        {
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Contest cannot be published.");

            if (!_problems.Any())
                throw new InvalidOperationException("Contest must have at least one problem.");

            Status = ContestStatus.Published;
        }

        public void Archive()
        {
            if (Status != ContestStatus.Finished)
                throw new InvalidOperationException("Contest must be finished before archive.");

            Status = ContestStatus.Archived;
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
            if (Status != ContestStatus.Draft)
                throw new InvalidOperationException("Cannot update schedule after publish.");

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
    }
}