using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Contests.Domain.Aggregates;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Configurations
{
    public sealed class ContestConfiguration : IEntityTypeConfiguration<Contest>
    {
        public void Configure(EntityTypeBuilder<Contest> builder)
        {
            builder.ToTable("contests");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => ContestId.From(value));

            builder.Property(x => x.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .HasColumnName("start_time")
                .IsRequired();

            builder.Property(x => x.EndTime)
                .HasColumnName("end_time")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Visibility)
                .HasColumnName("visibility")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Type)
               .HasColumnName("type")
               .HasConversion<int>()
               .IsRequired();

            builder.Property(x => x.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.RegistrationStartTime)
                .HasColumnName("registration_start_time");

            builder.Property(x => x.RegistrationEndTime)
                .HasColumnName("registration_end_time");

            builder.Property(x => x.MaxParticipants)
                .HasColumnName("max_participants");

            builder.Property(x => x.IsLeaderboardFrozen)
                .HasColumnName("is_leaderboard_frozen");

            builder.Property(x => x.FreezeAt)
                .HasColumnName("freeze_at");

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at");

            builder.Property(x => x.AllowPractice)
                .HasColumnName("allow_practice");

            builder.Property(x => x.AllowVirtual)
                .HasColumnName("allow_virtual");

            builder.Property(x => x.TotalSubmissions)
                .HasColumnName("total_submissions");

            builder.HasMany(x => x.Problems)
                .WithOne()
                .HasForeignKey(x => x.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Participants)
                .WithOne()
                .HasForeignKey(x => x.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Submissions)
                .WithOne()
                .HasForeignKey(x => x.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.StartTime);
            builder.HasIndex(x => x.EndTime);
        }
    }
}