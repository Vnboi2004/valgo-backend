using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class InvalidPasswordResetTokenException : DomainException
    {
        public InvalidPasswordResetTokenException() : base("Invalid or expired password reset token.") { }
    }
}