using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        public string Value { get; } = null!;

        private Email() { }

        private Email(string value) => Value = value;

        public static Email Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailException("Email is required.");

            return new Email(value.Trim().ToLowerInvariant());
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}