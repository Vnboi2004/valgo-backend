using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class ProblemClassificationRefId : ValueObject
    {
        public Guid Value { get; }

        public ProblemClassificationRefId(Guid value) => Value = value;

        public static ProblemClassificationRefId New() => new(Guid.NewGuid());

        public static ProblemClassificationRefId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}