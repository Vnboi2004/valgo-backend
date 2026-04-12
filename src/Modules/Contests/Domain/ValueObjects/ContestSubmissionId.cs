using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Contests.Domain.ValueObjects
{
    public sealed class ContestSubmissionId : ValueObject
    {
        public Guid Value { get; }

        private ContestSubmissionId(Guid value) => Value = value;

        public static ContestSubmissionId New() => new(Guid.NewGuid());

        public static ContestSubmissionId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}