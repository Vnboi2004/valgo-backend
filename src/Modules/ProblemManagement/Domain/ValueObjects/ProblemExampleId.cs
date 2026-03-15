using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class ProblemExampleId : ValueObject
    {
        public Guid Value { get; }

        private ProblemExampleId() { }

        private ProblemExampleId(Guid value) => Value = value;

        public static ProblemExampleId New() => new(Guid.NewGuid());

        public static ProblemExampleId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}