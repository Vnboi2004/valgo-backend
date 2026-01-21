using VAlgo.Modules.ProblemManagement.Domain.Exceptions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.ProblemManagement.Domain.ValueObjects
{
    public sealed class AllowedLanguage : ValueObject
    {
        private string Value { get; } = null!;

        private AllowedLanguage() { }

        public AllowedLanguage(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidLanguageException();

            Value = value.Trim().ToLowerInvariant();
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}