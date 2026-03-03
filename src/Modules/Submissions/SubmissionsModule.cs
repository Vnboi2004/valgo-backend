using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Submissions.Application;
using VAlgo.Modules.Submissions.Infrastructure;

namespace VAlgo.Modules.Submissions
{
    public static class SubmissionsModule
    {
        public static IServiceCollection AddSubmissionsModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSubmissionApplication();
            services.AddSubmissionsInfrastructure(configuration);

            return services;
        }
    }
}