using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.ValueObjects
{
    public sealed class RefreshTokenId : ValueObject
    {
        public Guid Value { get; }

        private RefreshTokenId(Guid value) => Value = value;

        public static RefreshTokenId New() => new(Guid.NewGuid());

        public static RefreshTokenId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}