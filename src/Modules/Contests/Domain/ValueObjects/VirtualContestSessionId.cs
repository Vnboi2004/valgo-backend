using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.ValueObjects
{
    public sealed class VirtualContestSessionId : ValueObject
    {
        public Guid Value { get; }

        private VirtualContestSessionId(Guid value) => Value = value;

        public static VirtualContestSessionId New() => new(Guid.NewGuid());

        public static VirtualContestSessionId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}