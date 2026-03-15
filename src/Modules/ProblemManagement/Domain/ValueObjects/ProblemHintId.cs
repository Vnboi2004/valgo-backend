using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class ProblemHintId : ValueObject
    {
        public Guid Value { get; }

        private ProblemHintId(Guid value) => Value = value;

        public static ProblemHintId New() => new(Guid.NewGuid());

        public static ProblemHintId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}