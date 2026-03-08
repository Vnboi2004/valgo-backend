using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Contests.Application.Interfaces;
using VAlgo.Modules.Contests.Application.Leaderboard;
using VAlgo.Modules.Contests.Infrastructure.Leaderboard;
using VAlgo.Modules.Contests.Infrastructure.Persistence;
using VAlgo.Modules.Contests.Infrastructure.Persistence.Repositories;

namespace VAlgo.Modules.Contests.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContestsInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ContestsDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("ContestDb"), npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(ContestsDbContext).Assembly.FullName);
                    npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IContestRepository, ContestRepository>();

            // Register service
            services.AddScoped<ILeaderboardService, RedisLeaderboardService>();
            services.AddScoped<ILeaderboardCacheService, RedisLeaderboardCacheService>();

            return services;
        }
    }
}