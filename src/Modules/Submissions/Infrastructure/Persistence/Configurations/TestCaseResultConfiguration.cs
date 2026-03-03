using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Submissions.Domain.Entities;
using VAlgo.Modules.Submissions.Domain.ValueObjects;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Configurations
{
    public sealed class TestCaseResultConfiguration : IEntityTypeConfiguration<TestCaseResult>
    {
        public void Configure(EntityTypeBuilder<TestCaseResult> builder)
        {
            builder.ToTable("submission_test_case_results", "submissions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => TestCaseResultId.From(value))
                .ValueGeneratedNever();

            builder.Property(x => x.SubmissionId)
                .HasColumnName("submission_id")
                .IsRequired();

            builder.Property(x => x.TestCaseIndex)
                .HasColumnName("test_case_index")
                .IsRequired();

            builder.Property(x => x.Verdict)
                .HasColumnName("verdict")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.TimeMs)
                .HasColumnName("time_ms")
                .IsRequired();

            builder.Property(x => x.MemoryKb)
                .HasColumnName("memory_kb")
                .IsRequired();

            builder.Property(x => x.Output)
                .HasColumnName("output")
                .IsRequired(false);

            builder.HasIndex(x => x.SubmissionId)
                .HasDatabaseName("ix_test_case_results_submission");

            builder.HasIndex(x => new { x.SubmissionId, x.TestCaseIndex })
                .IsUnique()
                .HasDatabaseName("ux_test_case_results_submission_case");
        }
    }
}