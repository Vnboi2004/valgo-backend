using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class EmailNotVerifiedException : DomainException
    {
        public EmailNotVerifiedException() : base("Email has not been verified.") { }
    }
}