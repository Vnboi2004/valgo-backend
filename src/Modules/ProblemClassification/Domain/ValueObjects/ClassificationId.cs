using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemClassification.Domain.ValueObjects
{
    public sealed class ClassificationId : ValueObject
    {
        public Guid Value { get; }

        private ClassificationId(Guid value) => Value = value;

        public static ClassificationId New() => new(Guid.NewGuid());

        public static ClassificationId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}