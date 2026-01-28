using VAlgo.SharedKernel.Domain;

namespace VAlgo.Modules.Identity.Domain.Exceptions
{
    public sealed class UserLockedException : DomainException
    {
        public UserLockedException() : base("UserLockedException.") { }
    }
}