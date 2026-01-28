using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence.Configurations
{
    public sealed class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
    {
        public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
        {
            builder.ToTable("email_verification_tokens");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => EmailVerificationTokenId.From(value));

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .HasConversion(id => id.Value, value => UserId.From(value))
                .IsRequired();

            builder.Property(x => x.Token)
                .HasColumnName("token")
                .IsRequired();

            builder.Property(x => x.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            builder.Property(x => x.IsUsed)
                .HasColumnName("is_used")
                .IsRequired();

            builder.HasIndex(x => x.Token)
                .IsUnique();

            builder.HasIndex(x => x.UserId);
        }
    }
}