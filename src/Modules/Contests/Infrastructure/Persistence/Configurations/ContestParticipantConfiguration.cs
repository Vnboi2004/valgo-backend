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

            builder.Property(x => x.Rank)
                .HasColumnName("rank")
                .IsRequired();

            builder.Property(x => x.LastSubmissionAt)
                .HasColumnName("last_submission_at")
                .IsRequired(false);

            builder.HasIndex(x => x.ContestId);

            builder.HasIndex(x => new { x.ContestId, x.UserId })
                .IsUnique();
        }
    }
}