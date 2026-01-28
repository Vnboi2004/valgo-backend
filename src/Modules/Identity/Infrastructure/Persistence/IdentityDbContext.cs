using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.Entities;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence
{
    public sealed class IdentityDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<EmailVerificationToken> EmailVerificationTokens => Set<EmailVerificationToken>();
        public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
        public DbSet<LoginAttempt> LoginAttempts => Set<LoginAttempt>();

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}