using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class InvalidRefreshTokenException : DomainException
    {
        public InvalidRefreshTokenException() : base("Invalid refresh token.") { }
    }
}