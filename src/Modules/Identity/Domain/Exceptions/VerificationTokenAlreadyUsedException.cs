using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Domain.Exceptions
{
    public sealed class VerificationTokenAlreadyUsedException : DomainException
    {
        public VerificationTokenAlreadyUsedException() : base("Email verification token has already been used.") { }
    }
}