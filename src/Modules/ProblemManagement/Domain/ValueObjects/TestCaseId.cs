using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class TestCaseId : ValueObject
    {
        public Guid Value { get; }

        private TestCaseId(Guid value) => Value = value;

        public static TestCaseId New() => new(Guid.NewGuid());

        public static TestCaseId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}