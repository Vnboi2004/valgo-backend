using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.ProblemClassification.Application;
using VAlgo.Modules.ProblemClassification.Application.Abstractions;
using VAlgo.Modules.ProblemClassification.Infrastructure.Persistence;
using VAlgo.Modules.ProblemClassification.Infrastructure.Persistence.Repositories;
using VAlgo.Modules.ProblemClassification.Infrastructure.Read;
using VAlgo.Modules.ProblemClassification.Infrastructure.Time;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.ProblemClassification.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClassificationInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ClassificationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ClassificationDb"), npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(ClassificationDbContext).Assembly.FullName);

                    npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClock, SystemClock>();
            services.AddScoped<IClassificationRepository, ClassificationRepository>();
            services.AddScoped<IClassificationQueries, ClassificationQueries>();

            return services;
        }
    }
}