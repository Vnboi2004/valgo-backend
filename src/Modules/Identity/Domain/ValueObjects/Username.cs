using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.ValueObjects
{
    public sealed class Username : ValueObject
    {
        public string Value { get; } = null!;

        private Username() { }

        private Username(string value) => Value = value;

        public static Username Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidUsernameException();

            return new Username(value.Trim());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}