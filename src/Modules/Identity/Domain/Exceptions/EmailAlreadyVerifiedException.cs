using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Domain.Exceptions
{
    public sealed class EmailAlreadyVerifiedException : DomainException
    {
        public EmailAlreadyVerifiedException() : base("Email has already been verified.") { }
    }
}