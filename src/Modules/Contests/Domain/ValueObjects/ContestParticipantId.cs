using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.ValueObjects
{
    public sealed class ContestParticipantId : ValueObject
    {
        public Guid Value { get; }

        private ContestParticipantId(Guid value) => Value = value;

        public static ContestParticipantId New() => new(Guid.NewGuid());

        public static ContestParticipantId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}