using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class ProblemCodeTemplateId : ValueObject
    {
        public Guid Value { get; }

        public ProblemCodeTemplateId(Guid value) => Value = value;

        public static ProblemCodeTemplateId New() => new(Guid.NewGuid());

        public static ProblemCodeTemplateId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}