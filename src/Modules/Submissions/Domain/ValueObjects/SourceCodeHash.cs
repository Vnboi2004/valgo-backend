using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using VAlgo.SharedKernel.Abstractions;

namespace VAlgo.Modules.Submissions.Domain.ValueObjects
{
    public sealed class SourceCodeHash : ValueObject
    {
        public string Value { get; }

        private SourceCodeHash(string value) => Value = value;

        public static SourceCodeHash From(string sourceCode)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(sourceCode);
            var hash = sha256.ComputeHash(bytes);

            return new SourceCodeHash(Convert.ToHexString(hash));
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}