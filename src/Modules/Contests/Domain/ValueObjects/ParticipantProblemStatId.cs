using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.ValueObjects
{
    public sealed class ParticipantProblemStatId : ValueObject
    {
        public Guid Value { get; }

        private ParticipantProblemStatId(Guid value) => Value = value;

        public static ParticipantProblemStatId New() => new(Guid.NewGuid());

        public static ParticipantProblemStatId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}