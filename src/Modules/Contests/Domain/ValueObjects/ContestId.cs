using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.ValueObjects
{
    public sealed class ContestId : ValueObject
    {
        public Guid Value { get; }

        private ContestId(Guid value) => Value = value;

        public static ContestId New() => new(Guid.NewGuid());

        public static ContestId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}