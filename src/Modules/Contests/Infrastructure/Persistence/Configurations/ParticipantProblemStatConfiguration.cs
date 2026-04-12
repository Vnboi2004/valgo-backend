using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Contests.Domain.Entities;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Configurations
{
    public sealed class ParticipantProblemStatConfiguration : IEntityTypeConfiguration<ParticipantProblemStat>
    {
        public void Configure(EntityTypeBuilder<ParticipantProblemStat> builder)
        {
            builder.ToTable("participant_problem_stats");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => ParticipantProblemStatId.From(value));

            builder.Property(x => x.ContestParticipantId)
                .HasColumnName("contest_participant_id")
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value != null ? ContestParticipantId.From(value.Value) : null
                )
                .IsRequired(false);

            builder.Property(x => x.VirtualContestSessionId)
                .HasColumnName("virtual_contest_session_id")
                .HasConversion(
                    id => id != null ? id.Value : (Guid?)null,
                    value => value != null ? VirtualContestSessionId.From(value.Value) : null
                )
                .IsRequired(false);

            builder.Property(x => x.ProblemId)
                .HasColumnName("problem_id")
                .IsRequired();

            builder.Property(x => x.IsSolved)
                .HasColumnName("is_solved")
                .IsRequired();

            builder.Property(x => x.WrongAttempts)
                .HasColumnName("wrong_attempts")
                .IsRequired();

            builder.Property(x => x.SolvedAt)
                .HasColumnName("solved_at")
                .IsRequired(false);

            // For contest
            builder.HasIndex(x => new { x.ContestParticipantId, x.ProblemId })
                .IsUnique()
                .HasFilter("\"contest_participant_id\" IS NOT NULL");

            // For virtual
            builder.HasIndex(x => new { x.VirtualContestSessionId, x.ProblemId })
                .IsUnique()
                .HasFilter("\"virtual_contest_session_id\" IS NOT NULL");
        }
    }
}