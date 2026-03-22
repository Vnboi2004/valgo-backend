namespace VAlgo.Modules.UserProfile.Application.Exceptions
{
    public sealed class UserProfileNotFoundException : Exception
    {
        public UserProfileNotFoundException(string username) : base($"User '{username}' was not found.") { }
    }
}