using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.Exceptions
{
    public sealed class PasswordHash : ValueObject
    {
        public string Value { get; }

        private PasswordHash(string value) => Value = value;

        public static PasswordHash FromHashed(string hashed)
            => new PasswordHash(hashed);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}