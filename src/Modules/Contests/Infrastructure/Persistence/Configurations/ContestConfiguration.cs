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

            builder.Property(x => x.MaxParticipants)
                .HasColumnName("max_participants")
                .IsRequired(false);

            builder.Property(x => x.CreatedBy)
                .HasColumnName("created_by")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.HasMany(x => x.Problems)
                .WithOne()
                .HasForeignKey(x => x.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Participants)
                .WithOne()
                .HasForeignKey(x => x.ContestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.StartTime);
            builder.HasIndex(x => x.EndTime);
        }
    }
}