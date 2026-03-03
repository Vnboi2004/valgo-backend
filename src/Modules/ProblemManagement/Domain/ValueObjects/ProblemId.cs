using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class ProblemId : ValueObject
    {
        public Guid Value { get; }

        private ProblemId(Guid value) => Value = value;

        public static ProblemId New() => new(Guid.NewGuid());

        public static ProblemId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}