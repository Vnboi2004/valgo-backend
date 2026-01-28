using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class InvalidCredentialsException : DomainException
    {
        public InvalidCredentialsException() : base("Invalid email or password.") { }
    }
}