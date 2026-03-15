using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class ProblemCompanyRefId : ValueObject
    {
        public Guid Value { get; }

        private ProblemCompanyRefId(Guid value) => Value = value;

        public static ProblemCompanyRefId New() => new(Guid.NewGuid());

        public static ProblemCompanyRefId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}