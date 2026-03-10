using VAlgo.Modules.Identity.Domain.Enums;
using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Domain.Aggregates
{
    public sealed class User : AggregateRoot<UserId>
    {
        public Email Email { get; private set; } = null!;
        public Username Username { get; private set; } = null!;
        public PasswordHash PasswordHash { get; private set; } = null!;
        public UserRole Role { get; private set; }
        public UserStatus Status { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? LockedUntil { get; private set; }

        private User() { }

        private User(UserId id, Email email, Username username, PasswordHash passwordHash) : base(id)
        {
            Email = email;
            Username = username;
            PasswordHash = passwordHash;
            Role = UserRole.User;
            Status = UserStatus.EmailNotVerified;
            IsEmailVerified = false;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public static User Register(Email email, Username username, PasswordHash passwordHash)
        {
            return new User(UserId.New(), email, username, passwordHash);
        }

        public void VerifyEmail()
        {
            if (IsEmailVerified)
                throw new EmailAlreadyVerifiedException();

            IsEmailVerified = true;
            Status = UserStatus.Active;
        }

        public void ChangePassword(PasswordHash newPasswordHash)
        {
            if (Status == UserStatus.Locked)
                throw new UserLockedException();

            PasswordHash = newPasswordHash;
        }

        public void PromoteToAdmin()
        {
            Role = UserRole.Admin;
        }

        public void Lock(DateTimeOffset until)
        {
            Status = UserStatus.Locked;
            LockedUntil = until;
        }

        public bool IsLocked(IClock clock)
        {
            if (Status != UserStatus.Locked)
                return false;

            if (LockedUntil <= clock.UtcNow)
            {
                Status = UserStatus.Active;
                LockedUntil = null;
                return false;
            }

            return true;
        }

        public void Deactivate()
        {
            Status = UserStatus.Deactivated;
        }
    }
}