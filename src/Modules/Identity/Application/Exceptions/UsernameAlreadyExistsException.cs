using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class UsernameAlreadyExistsException : DomainException
    {
        public UsernameAlreadyExistsException(string username) : base($"Username '{username}' is already in use.") { }
    }
}