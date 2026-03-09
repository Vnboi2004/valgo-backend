using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Discussions.Application.Interfaces;
using VAlgo.Modules.Discussions.Infrastructure.Persistence;
using VAlgo.Modules.Discussions.Infrastructure.Persistence.Repositories;

namespace VAlgo.Modules.Discussions.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDiscussionsInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DiscussionsDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DiscussionDb"), npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(DiscussionsDbContext).Assembly.FullName);
                    npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDiscussionRepository, DiscussionRepository>();

            return services;
        }
    }
}