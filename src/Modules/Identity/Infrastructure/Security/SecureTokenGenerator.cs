using System.Security.Cryptography;
using VAlgo.Modules.Identity.Application.Abstractions.Security;

namespace VAlgo.Modules.Identity.Infrastructure.Security
{
    public sealed class SecureTokenGenerator : ISecureTokenGenerator
    {
        public string Generate(int length = 64)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);

            return Convert.ToBase64String(bytes)
                .Replace("+", string.Empty)
                .Replace("/", string.Empty)
                .Replace("=", string.Empty);
        }
    }
}