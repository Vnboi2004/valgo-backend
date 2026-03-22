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
        public UserRole Role { get; private set; }
        public UserStatus Status { get; private set; }

        // General Info
        public string? DisplayName { get; private set; }
        public string? Avatar { get; private set; }
        public Gender? Gender { get; private set; }
        public string? Location { get; private set; }
        public DateOnly? Birthday { get; private set; }
        public string? Website { get; private set; }
        public string? Github { get; private set; }
        public string? LinkedIn { get; private set; }
        public string? Twitter { get; private set; }
        public string? ReadMe { get; private set; }

        // Experience
        public string? Work { get; private set; }
        public string? Education { get; private set; }

        // Privacy settings
        public bool ShowRecentSubmissions { get; private set; }
        public bool ShowSubmissionHeatmap { get; private set; }

        public PasswordHash PasswordHash { get; private set; } = null!;
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

        public void UpdateProfile(
            string? displayName,
            Gender? gender,
            string? location,
            DateOnly? birthday,
            string? website,
            string? github,
            string? linkedIn,
            string? twitter,
            string? readMe)
        {
            if (Status == UserStatus.Deactivated)
                throw new InvalidOperationException("User deactivated.");

            DisplayName = displayName;
            Gender = gender;
            Location = location;
            Birthday = birthday;
            Website = website;
            Github = github;
            LinkedIn = linkedIn;
            Twitter = twitter;
            ReadMe = readMe;
        }

        public void UpdateExperience(string? work, string? education)
        {
            if (Status == UserStatus.Deactivated)
                throw new InvalidOperationException("User deactivated.");

            Work = work;
            Education = education;
        }

        public void UpdatePrivacySettings(bool showRecentSubmissions, bool showSubmissionHeatmap)
        {
            ShowRecentSubmissions = showRecentSubmissions;
            ShowSubmissionHeatmap = showSubmissionHeatmap;
        }

        public void UpdateAvatar(string avatarUrl)
        {
            if (string.IsNullOrWhiteSpace(avatarUrl))
                throw new InvalidOperationException("Invalid avatar.");

            Avatar = avatarUrl;
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