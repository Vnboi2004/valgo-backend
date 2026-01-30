using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.ValueObjects
{
    public sealed class TestCaseResultId : ValueObject
    {
        public Guid Value { get; }

        private TestCaseResultId(Guid value) => Value = value;

        public static TestCaseResultId New() => new(Guid.NewGuid());

        public static TestCaseResultId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}