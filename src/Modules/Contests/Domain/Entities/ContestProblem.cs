using System.Drawing;
using VAlgo.Modules.Contests.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.Entities
{
    public sealed class ContestProblem : Entity<ContestProblemId>
    {
        public ContestId ContestId { get; private set; } = null!;
        public Guid ProblemId { get; private set; }
        public string Code { get; private set; } = null!;
        public int Order { get; private set; }
        public int Points { get; private set; }

        private ContestProblem() { }

        private ContestProblem(ContestProblemId id, ContestId contestId, Guid problemId, string code, int order, int points)
            : base(id)
        {
            ContestId = contestId;
            ProblemId = problemId;
            Code = code;
            Order = order;
            Points = points;
        }

        public static ContestProblem Create(ContestId contestId, Guid problemId, string code, int order, int points)
            => new ContestProblem(ContestProblemId.New(), contestId, problemId, code, order, points);

        public void UpdateOrder(int order)
        {
            Order = order;
        }

        public void UpdatePoints(int points)
        {
            Points = points;
        }
    }
}