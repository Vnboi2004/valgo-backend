using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Discussions.Application;
using VAlgo.Modules.Discussions.Infrastructure;

namespace VAlgo.Modules.Discussions
{
    public static class DiscussionsModule
    {
        public static IServiceCollection AddDiscussionsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDiscussionsApplication();
            services.AddDiscussionsInfrastructure(configuration);

            return services;
        }
    }
}