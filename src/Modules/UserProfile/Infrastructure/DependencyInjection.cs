using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.UserProfile.Application.Abstractions;
using VAlgo.Modules.UserProfile.Infrastructure.Services;

namespace VAlgo.Modules.UserProfile.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserProfileInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserIdentityReadService, UserIdentityReadService>();
            services.AddScoped<IUserProfileReadService, UserProfileReadService>();

            return services;
        }
    }
}