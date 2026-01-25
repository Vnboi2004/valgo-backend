using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Domain.Exceptions
{
    public sealed class InvalidEmailException : DomainException
    {
        public InvalidEmailException(string value) : base($"Email '{value}' is invalid.") { }
    }
}