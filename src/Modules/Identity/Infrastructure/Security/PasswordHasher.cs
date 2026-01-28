using VAlgo.Modules.Identity.Application.Abstractions.Security;

namespace VAlgo.Modules.Identity.Infrastructure.Security
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 12;

        public string Hash(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: WorkFactor);
        }
        public bool Verify(string plainPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
        }
    }
}