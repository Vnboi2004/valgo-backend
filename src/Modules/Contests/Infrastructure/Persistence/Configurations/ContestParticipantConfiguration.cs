using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Contests.Domain.Entities;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Configurations
{
    public sealed class ContestParticipantConfiguration : IEntityTypeConfiguration<ContestParticipant>
    {
        public void Configure(EntityTypeBuilder<ContestParticipant> builder)
        {
            builder.ToTable("contest_participants");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => ContestParticipantId.From(value));

            builder.Property(x => x.ContestId)
                .HasColumnName("contest_id")
                .HasConversion(x => x.Value, value => ContestId.From(value))
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.JoinedAt)
                .HasColumnName("joined_at")
                .IsRequired();

            builder.Property(x => x.Score)
                .HasColumnName("score")
                .IsRequired();

            builder.Property(x => x.Penalty)
                .HasColumnName("penalty")
                .IsRequired();

            builder.Property(x => x.SubmissionCount)
                .HasColumnName("submission_count");

            builder.Property(x => x.IsRegistered)
                .HasColumnName("is_registered");

            builder.Property(x => x.RegisteredAt)
                .HasColumnName("registered_at");

            builder.HasMany(x => x.ProblemStats)
                .WithOne()
                .HasForeignKey(x => x.ContestParticipantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.ContestId);

            builder.HasIndex(x => new { x.ContestId, x.UserId })
                .IsUnique();
        }
    }
}