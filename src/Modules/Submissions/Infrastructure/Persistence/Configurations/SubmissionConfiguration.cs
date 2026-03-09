using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Submissions.Domain.Aggregates;
using VAlgo.Modules.Submissions.Domain.Entities;
using VAlgo.Modules.Submissions.Domain.ValueObjects;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Configurations
{
    public sealed class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.ToTable("submissions", schema: "submissions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => SubmissionId.From(value))
                .ValueGeneratedNever();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.ProblemId)
                .HasColumnName("problem_id")
                .IsRequired();

            builder.Property(x => x.SourceCode)
                .HasColumnName("source_code")
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.Property(x => x.QueuedAt)
                .HasColumnName("queued_at")
                .IsRequired(false);

            builder.Property(x => x.StartedAt)
                .HasColumnName("started_at")
                .IsRequired(false);

            builder.Property(x => x.FinishedAt)
                .HasColumnName("finished_at")
                .IsRequired(false);

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.Verdict)
                .HasColumnName("verdict")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.FailureReason)
                .HasColumnName("failure_reason")
                .HasConversion<int>()
                .IsRequired(false);

            builder.Property(x => x.RetryCount)
                .HasColumnName("retry_count")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.WorkerId)
                .HasColumnName("worker_id")
                .HasMaxLength(128)
                .IsRequired(false);

            builder.OwnsOne(x => x.Language, lang =>
            {
                lang.Property(l => l.Value)
                    .HasColumnName("language")
                    .HasMaxLength(50)
                    .IsRequired();
            });

            builder.OwnsOne(x => x.SourceCodeHash, hash =>
            {
                hash.Property(h => h.Value)
                    .HasColumnName("source_code_hash")
                    .HasMaxLength(64)
                    .IsRequired();
            });

            builder.OwnsOne(x => x.JudgeSummary, summary =>
            {
                summary.Property(s => s.PassedTestCases)
                    .HasColumnName("passed_test_cases");

                summary.Property(s => s.TotalTestCases)
                    .HasColumnName("total_test_cases");

                summary.Property(s => s.MaxTimeMs)
                    .HasColumnName("max_time_ms");

                summary.Property(s => s.MaxMemoryKb)
                    .HasColumnName("max_memory_kb");
            });

            builder.Navigation(x => x.JudgeSummary)
                .IsRequired(false);

            builder.HasMany(x => x.TestCaseResults)
                .WithOne()
                .HasForeignKey(x => x.SubmissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Navigation(x => x.TestCaseResults)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasIndex(x => x.UserId)
                .HasDatabaseName("idx_submissions_user_id");

            builder.HasIndex(x => x.ProblemId)
                .HasDatabaseName("idx_submissions_problem_id");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("idx_submissions_status");

            builder.HasIndex(x => x.CreatedAt)
                .HasDatabaseName("idx_submissions_created_at");

            builder.HasIndex(x => new { x.Status, x.WorkerId })
                .HasDatabaseName("ix_submissions_status_worker");

            builder.HasIndex(x => new { x.Status, x.RetryCount })
                .HasDatabaseName("ix_submissions_retry");
        }
    }
}