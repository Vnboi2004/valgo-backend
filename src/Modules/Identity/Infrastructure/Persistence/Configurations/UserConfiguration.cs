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
            });

            builder.OwnsOne(x => x.Username, username =>
            {
                username.Property(u => u.Value)
                    .HasColumnName("username")
                    .HasMaxLength(100)
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

            builder.Property(x => x.DisplayName)
                .HasColumnName("display_name")
                .IsRequired(false);

            builder.Property(x => x.Avatar)
                .HasColumnName("avatar")
                .IsRequired(false);

            builder.Property(x => x.Gender)
                .HasColumnName("gender")
                .HasConversion<int>()
                .IsRequired(false);

            builder.Property(x => x.Location)
                .HasColumnName("location")
                .IsRequired(false);

            builder.Property(x => x.Birthday)
                .HasColumnName("birthday")
                .IsRequired(false);

            builder.Property(x => x.Website)
                .HasColumnName("website")
                .IsRequired(false);

            builder.Property(x => x.Github)
                .HasColumnName("github")
                .IsRequired(false);

            builder.Property(x => x.LinkedIn)
                .HasColumnName("linked_in")
                .IsRequired(false);

            builder.Property(x => x.Twitter)
                .HasColumnName("twitter")
                .IsRequired(false);

            builder.Property(x => x.ReadMe)
                .HasColumnName("read_me")
                .IsRequired(false);

            builder.Property(x => x.Work)
                .HasColumnName("work")
                .IsRequired(false);

            builder.Property(x => x.Education)
                .HasColumnName("education")
                .IsRequired(false);

            builder.Property(x => x.ShowRecentSubmissions)
                .HasColumnName("show_recent_submissions")
                .IsRequired();

            builder.Property(x => x.ShowSubmissionHeatmap)
                .HasColumnName("show_submission_heatmap")
                .IsRequired();

            builder.OwnsOne(x => x.PasswordHash, password =>
            {
                password.Property(p => p.Value)
                    .HasColumnName("password_hash")
                    .IsRequired();
            });

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