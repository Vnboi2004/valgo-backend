using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class InvalidCurrentPasswordException : DomainException
    {
        public InvalidCurrentPasswordException() : base("Current password is incorrect.") { }
    }
}