using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class UserNotFoundException : DomainException
    {
        public UserNotFoundException(Guid userId) : base($"User with id '{userId}' was not found.") { }
    }
}