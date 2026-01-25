using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Domain.Exceptions
{
    public sealed class InvalidUsernameException : DomainException
    {
        public InvalidUsernameException() : base("Username is invalid.") { }
    }
}