using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Application.Abstractions;
using VAlgo.Modules.Identity.Application.Queries.GetMyProfile;
using VAlgo.Modules.Identity.Application.Queries.GetUserPublicProfile;
using VAlgo.Modules.Identity.Domain.Enums;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.Modules.Identity.Infrastructure.Persistence;

namespace VAlgo.Modules.Identity.Infrastructure.Read
{
    public sealed class UserReadRepository : IUserReadRepository
    {
        private readonly IdentityDbContext _dbContext;

        public UserReadRepository(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<MyProfileDto?> GetMyProfileAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Where(u => u.Id == UserId.From(userId))
                .Select(u => new MyProfileDto
                {
                    UserId = u.Id.Value,
                    Username = u.Username.Value,
                    Email = u.Email.Value,
                    DisplayName = u.DisplayName,
                    Avatar = u.Avatar,
                    Gender = u.Gender,
                    Location = u.Location,
                    Birthday = u.Birthday,
                    Website = u.Website,
                    Github = u.Github,
                    LinkedIn = u.LinkedIn,
                    Twitter = u.Twitter,
                    ReadMe = u.ReadMe,
                    Work = u.Work,
                    Education = u.Education,
                    ShowRecentSubmissions = u.ShowRecentSubmissions,
                    ShowSubmissionHeatmap = u.ShowSubmissionHeatmap,
                    Role = u.Role,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<UserPublicProfileDto?> GetPublicProfileAsync(
            string username,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Where(u =>
                    u.Username.Value == username &&
                    u.Status == UserStatus.Active)
                .Select(u => new UserPublicProfileDto
                {
                    UserId = u.Id.Value,
                    Username = u.Username.Value,
                    DisplayName = u.DisplayName,
                    Avatar = u.Avatar,
                    Location = u.Location,
                    Website = u.Website,
                    Github = u.Github,
                    LinkedIn = u.LinkedIn,
                    Twitter = u.Twitter,
                    ReadMe = u.ReadMe,
                    Work = u.Work,
                    Education = u.Education,
                    ShowRecentSubmissions = u.ShowRecentSubmissions,
                    ShowSubmissionHeatmap = u.ShowSubmissionHeatmap,
                    CreatedAt = u.CreatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}