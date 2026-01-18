using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.ValueObjects
{
    public sealed class SubmissionId : ValueObject
    {
        public Guid Value { get; }

        private SubmissionId(Guid value) => Value = value;

        public static SubmissionId New() => new(Guid.NewGuid());

        public static SubmissionId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}