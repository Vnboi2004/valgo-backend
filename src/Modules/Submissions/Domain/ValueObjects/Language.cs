using VAlgo.Modules.Submissions.Domain.Exceptions;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.ValueObjects
{
    public sealed class Language : ValueObject
    {
        public string Value { get; }

        public Language(string value)
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