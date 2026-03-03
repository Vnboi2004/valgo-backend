using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VAlgo.Modules.Identity.Application.Abstractions.Communication;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Abstractions.Security;
using VAlgo.Modules.Identity.Application.Persistence;
using VAlgo.Modules.Identity.Application.Policies;
using VAlgo.Modules.Identity.Domain.Services;
using VAlgo.Modules.Identity.Infrastructure.Communication;
using VAlgo.Modules.Identity.Infrastructure.Persistence;
using VAlgo.Modules.Identity.Infrastructure.Persistence.Repositories;
using VAlgo.Modules.Identity.Infrastructure.Security;
using VAlgo.Modules.Identity.Infrastructure.Time;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("IdentityDb"), npgsql =>
                {
                    npgsql.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName);
                    npgsql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorCodesToAdd: null);
                });
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IClock, SystemClock>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            services.AddScoped<ILoginAttemptRepository, LoginAttemptRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IAccessTokenGenerator, JwtAccessTokenGenerator>();
            services.AddScoped<ISecureTokenGenerator, SecureTokenGenerator>();
            services.AddScoped<ILoginPolicy, LoginPolicy>();
            services.AddScoped<IEmailSender, SmtpEmailSender>();

            services.Configure<EmailOptions>(configuration.GetSection("Email"));
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));

            return services;
        }
    }
}