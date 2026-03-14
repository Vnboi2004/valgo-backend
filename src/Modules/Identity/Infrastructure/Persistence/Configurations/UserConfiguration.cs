using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence.Configurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => UserId.From(value));

            builder.OwnsOne(x => x.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsRequired();

                // email.HasIndex(e => e.Value)
                //     .IsUnique();
            });

            builder.OwnsOne(x => x.Username, username =>
            {
                username.Property(u => u.Value)
                    .HasColumnName("username")
                    .HasMaxLength(100)
                    .IsRequired();

                // username.HasIndex(u => u.Value)
                //     .IsUnique();
            });

            builder.OwnsOne(x => x.PasswordHash, password =>
            {
                password.Property(p => p.Value)
                    .HasColumnName("password_hash")
                    .IsRequired();
            });

            builder.Property(x => x.Role)
                .HasColumnName("role")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.IsEmailVerified)
                .HasColumnName("is_email_verified")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.LockedUntil)
                .HasColumnName("locked_until")
                .IsRequired(false);
        }
    }
}