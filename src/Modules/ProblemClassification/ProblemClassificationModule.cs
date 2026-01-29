using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.ProblemClassification.Application;
using VAlgo.Modules.ProblemClassification.Infrastructure;

namespace VAlgo.Modules.ProblemClassification
{
    public static class ProblemClassificationModule
    {
        public static IServiceCollection AddProblemClassificationModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddClassificationApplication();
            services.AddClassificationInfrastructure(configuration);

            return services;
        }
    }
}