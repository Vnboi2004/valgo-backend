using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class CompanyId : ValueObject
    {
        public Guid Value { get; }

        private CompanyId(Guid value) => Value = value;

        public static CompanyId New() => new(Guid.NewGuid());

        public static CompanyId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}