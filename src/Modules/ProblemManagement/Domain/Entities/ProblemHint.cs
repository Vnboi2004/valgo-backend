using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Entities
{
    public sealed class ProblemHint : Entity<ProblemHintId>
    {
        public int Order { get; private set; }
        public string Content { get; private set; } = null!;

        private ProblemHint() { }

        private ProblemHint(ProblemHintId id, int order, string content)
            : base(id)
        {
            Order = order;
            Content = content;
        }

        public static ProblemHint Create(int order, string content)
        {
            return new ProblemHint(ProblemHintId.New(), order, content);
        }

        public void Update(string content)
        {
            Content = content;
        }

        public void SetOrder(int order)
        {
            Order = order;
        }
    }
}