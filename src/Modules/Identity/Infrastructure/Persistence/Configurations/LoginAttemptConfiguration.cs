using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Infrastructure.Persistence.Configurations
{
    public sealed class LoginAttemptConfiguration : IEntityTypeConfiguration<LoginAttempt>
    {
        public void Configure(EntityTypeBuilder<LoginAttempt> builder)
        {
            builder.ToTable("login_attempts");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => LoginAttemptId.From(value));

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .HasConversion(id => id!.Value, value => UserId.From(value))
                .IsRequired(false);

            builder.OwnsOne(x => x.Email, email =>
            {
                email.Property(e => e.Value)
                    .HasColumnName("email")
                    .HasMaxLength(255);
            });

            builder.Property(x => x.Result)
                .HasColumnName("result")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.FailureReason)
                .HasColumnName("failure_reason")
                .HasConversion<int>()
                .IsRequired(false);

            builder.Property(x => x.OccurredAt)
                .HasColumnName("occurred_at")
                .IsRequired();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.OccurredAt);
            builder.HasIndex(x => new { x.UserId, x.Result, x.OccurredAt });
        }
    }
}