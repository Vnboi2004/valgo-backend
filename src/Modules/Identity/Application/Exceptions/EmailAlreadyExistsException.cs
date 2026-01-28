using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class EmailAlreadyExistsException : DomainException
    {
        public EmailAlreadyExistsException(string email) : base($"Email '{email} is already in use.'") { }
    }
}