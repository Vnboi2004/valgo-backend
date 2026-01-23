using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.ProblemManagement.Application.Abstractions;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence;
using VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Repositories;
using VAlgo.Modules.ProblemManagement.Infractructure.Read;
using VAlgo.Modules.ProblemManagement.Infractructure.Time;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.ProblemManagement.Infractructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProblemManagementInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProblemManagementDbContext>(options =>
            {
                services.AddDbContext<ProblemManagementDbContext>(options =>
                {
                    options.UseNpgsql(configuration.GetConnectionString("ProblemManagementDb"), npgsql =>
                    {
                        npgsql.MigrationsAssembly(typeof(ProblemManagementDbContext).Assembly.FullName);

                        npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                    });
                });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProblemRepository, ProblemRepository>();
            services.AddScoped<IProblemManagementQueries, ProblemManagementQueries>();
            services.AddScoped<IClock, SystemClock>();

            return services;
        }
    }
}