using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.ValueObjects
{
    public sealed class PasswordResetTokenId : ValueObject
    {
        public Guid Value { get; }

        private PasswordResetTokenId(Guid value) => Value = value;

        public static PasswordResetTokenId New() => new(Guid.NewGuid());

        public static PasswordResetTokenId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}