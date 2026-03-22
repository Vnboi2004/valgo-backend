using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Identity.Application;
using VAlgo.Modules.UserProfile.Application;
using VAlgo.Modules.UserProfile.Infrastructure;

namespace VAlgo.Modules.UserProfile
{
    public static class UserProfileModule
    {
        public static IServiceCollection AddUserProfileModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddUserProfileApplication();
            services.AddUserProfileInfrastructure(configuration);

            return services;
        }
    }
}