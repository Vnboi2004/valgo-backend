using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.ValueObjects
{
    public sealed class ExternalLoginId : ValueObject
    {
        public Guid Value { get; }

        private ExternalLoginId(Guid value) => Value = value;

        public static ExternalLoginId New() => new(Guid.NewGuid());

        public static ExternalLoginId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}