using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Submissions.Domain.Aggregates;
using VAlgo.Modules.Submissions.Domain.ValueObjects;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Configurations
{
    public sealed class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.ToTable("Submissions", schema: "submissions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(id => id.Value, value => SubmissionId.From(value))
                .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.ProblemId)
                .IsRequired();

            builder.Property(x => x.SourceCode)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.QueuedAt);

            builder.Property(x => x.StartedAt);

            builder.Property(x => x.FinishedAt);

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Verdict)
                .HasConversion<int>()
                .IsRequired();

            builder.OwnsOne(x => x.Language, lang =>
            {
                lang.Property(l => l.Value)
                    .HasColumnName("Language")
                    .HasMaxLength(50)
                    .IsRequired();
            });

            builder.OwnsOne(x => x.JudgeSummary, summary =>
            {
                summary.Property(s => s.PassedTestCases)
                    .HasColumnName("PassedTestcases");

                summary.Property(s => s.TotalTestCases)
                    .HasColumnName("TotalTestCases");

                summary.Property(s => s.MaxTimeMs)
                    .HasColumnName("MaxTimeMs");

                summary.Property(s => s.MaxMemoryKb)
                    .HasColumnName("MaxMemoryKb");
            });

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.ProblemId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.CreatedAt);
        }

    }
}