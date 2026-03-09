using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Contests.Application;
using VAlgo.Modules.Contests.Infrastructure;

namespace VAlgo.Modules.Contests
{
    public static class ContestsModule
    {
        public static IServiceCollection AddContestsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddContestsApplication();
            services.AddContestsInfrastructure(configuration);

            return services;
        }
    }
}