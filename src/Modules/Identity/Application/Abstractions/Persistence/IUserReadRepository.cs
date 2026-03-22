using VAlgo.Modules.Identity.Application.Queries.GetMyProfile;
using VAlgo.Modules.Identity.Application.Queries.GetUserPublicProfile;

namespace VAlgo.Modules.Identity.Application.Abstractions
{
    public interface IUserReadRepository
    {
        Task<MyProfileDto?> GetMyProfileAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserPublicProfileDto?> GetPublicProfileAsync(string username, CancellationToken cancellationToken);
    }
}