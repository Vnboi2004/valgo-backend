using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Identity.Domain.ValueObjects
{
    public sealed class LoginAttemptId : ValueObject
    {
        public Guid Value { get; }

        private LoginAttemptId(Guid value) => Value = value;

        public static LoginAttemptId New() => new(Guid.NewGuid());

        public static LoginAttemptId From(Guid value) => new(value);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}