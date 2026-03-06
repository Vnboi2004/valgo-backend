using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.ValueObjects
{
    public sealed class ContestProblemId : ValueObject
    {
        public Guid Value { get; }

        private ContestProblemId(Guid value) => Value = value;

        public static ContestProblemId New() => new(Guid.NewGuid());

        public static ContestProblemId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}