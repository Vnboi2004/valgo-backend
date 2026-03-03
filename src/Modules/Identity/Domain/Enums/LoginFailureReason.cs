namespace VAlgo.Modules.Identity.Domain.Enums
{
    public enum LoginFailureReason
    {
        InvalidCredentials = 1,
        EmailNotVerified = 2,
        UserLocked = 3,
        UserDeactivated = 4
    }
}