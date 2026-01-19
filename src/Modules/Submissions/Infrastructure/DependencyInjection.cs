using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Infrastructure.Integration;
using VAlgo.Modules.Submissions.Infrastructure.Persistence;
using VAlgo.Modules.Submissions.Infrastructure.Persistence.Repositories;
using VAlgo.Modules.Submissions.Infrastructure.Read;

namespace VAlgo.Modules.Submissions.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSubmissionsInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SubmissionsDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("SubmissionsDb"), npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(SubmissionsDbContext).Assembly.FullName);

                    npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            services.AddScoped<ISubmissionReadStore, SubmissionReadStore>();
            services.AddScoped<ISubmissionDetailReadStore, SubmissionDetailReadStore>();
            services.AddScoped<ISubmissionStatusReadStore, SubmissionStatusReadStore>();
            services.AddScoped<IJudgeResultReadStore, JudgeResultReadStore>();

            services.AddScoped<IJudgeQueue, JudgeQueue>();
            services.AddScoped<IUserReadService, UserReadService>();
            services.AddScoped<IProblemReadService, ProblemReadService>();

            return services;
        }
    }
}