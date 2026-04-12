using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Contests.Domain.Entities;
using VAlgo.Modules.Contests.Domain.ValueObjects;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence.Configurations
{
    public sealed class ContestSubmissionConfiguration : IEntityTypeConfiguration<ContestSubmission>
    {
        public void Configure(EntityTypeBuilder<ContestSubmission> builder)
        {
            builder.ToTable("contest_submissions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => ContestSubmissionId.From(value));

            builder.Property(x => x.ContestId)
                .HasColumnName("contest_id")
                .HasConversion(id => id.Value, value => ContestId.From(value))
                .IsRequired();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.ProblemId)
                .HasColumnName("problem_id")
                .IsRequired();

            builder.Property(x => x.Verdict)
                .HasColumnName("verdict")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.SubmittedAt)
                .HasColumnName("submitted_at")
                .IsRequired();

            builder.HasIndex(x => x.ContestId);
            builder.HasIndex(x => new { x.ContestId, x.UserId });
            builder.HasIndex(x => new { x.ContestId, x.ProblemId });
        }
    }
}