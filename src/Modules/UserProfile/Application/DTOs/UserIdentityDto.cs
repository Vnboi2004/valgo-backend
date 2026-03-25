namespace VAlgo.Modules.UserProfile.Application.DTOs
{
    public sealed class UserIdentityDto
    {
        public Guid Id { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}