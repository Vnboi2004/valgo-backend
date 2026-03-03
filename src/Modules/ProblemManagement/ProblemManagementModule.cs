using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.ProblemManagement.Application;
using VAlgo.Modules.ProblemManagement.Infractructure;

namespace VAlgo.Modules.ProblemManagement
{
    public static class ProblemManagementModule
    {
        public static IServiceCollection AddProblemManagementModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddProblemManagementApplication();
            services.AddProblemManagementInfrastructure(configuration);

            return services;
        }
    }
}