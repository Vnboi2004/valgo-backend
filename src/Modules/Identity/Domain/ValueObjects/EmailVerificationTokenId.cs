using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.ValueObjects
{
    public sealed class EmailVerificationTokenId : ValueObject
    {
        public Guid Value { get; }

        private EmailVerificationTokenId(Guid value) => Value = value;


        public static EmailVerificationTokenId New() => new(Guid.NewGuid());

        public static EmailVerificationTokenId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}