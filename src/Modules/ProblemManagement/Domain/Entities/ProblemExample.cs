using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.Entities
{
    public sealed class ProblemExample : Entity<ProblemExampleId>
    {
        public int Order { get; private set; }
        public string Input { get; private set; } = null!;
        public string Output { get; private set; } = null!;
        public string? Explanation { get; private set; }

        private ProblemExample() { }

        private ProblemExample(ProblemExampleId id, int order, string input, string output, string? explanation) : base(id)
        {
            Order = order;
            Input = input;
            Output = output;
            Explanation = explanation;
        }

        public static ProblemExample Create(int order, string input, string output, string? explanation)
        {
            return new ProblemExample(ProblemExampleId.New(), order, input, output, explanation);
        }

        public void Update(string input, string output, string? explanation)
        {
            Input = input;
            Output = output;
            Explanation = explanation;
        }

        public void SetOrder(int order)
        {
            Order = order;
        }
    }
}