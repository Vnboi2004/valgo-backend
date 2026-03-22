namespace VAlgo.Modules.UserProfile.Application.Abstractions
{
    public interface IUserIdentityReadService
    {
        Task<Guid?> GetUserIdByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid userId, CancellationToken cancellationToken);
    }
}