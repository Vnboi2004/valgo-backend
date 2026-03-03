using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Identity.Application;
using VAlgo.Modules.Identity.Infrastructure;

namespace VAlgo.Modules.Identity
{
    public static class IdentityModule
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityApplication();
            services.AddIdentityInfrastructure(configuration);

            return services;
        }
    }
}