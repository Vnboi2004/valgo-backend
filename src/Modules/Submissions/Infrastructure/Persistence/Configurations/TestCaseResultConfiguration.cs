using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Submissions.Infrastructure.Persistence.Entities;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Configurations
{
    public sealed class TestCaseResultConfiguration : IEntityTypeConfiguration<TestCaseResult>
    {
        public void Configure(EntityTypeBuilder<TestCaseResult> builder)
        {
            builder.ToTable("test_case_results", schema: "test_case_results");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            builder.Property(x => x.SubmissionId)
                .HasColumnName("submission_id")
                .IsRequired();

            builder.Property(x => x.Index)
                .HasColumnName("index")
                .IsRequired();

            builder.Property(x => x.Passed)
                .HasColumnName("passed")
                .IsRequired();

            builder.Property(x => x.TimeMs)
                .HasColumnName("time_ms")
                .IsRequired();

            builder.Property(x => x.MemoryKb)
                .HasColumnName("memory_kb")
                .IsRequired();

            builder.Property(x => x.Error)
                .HasColumnName("error")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            builder.HasIndex(x => x.SubmissionId)
                .HasDatabaseName("idx_test_case_results_submission_id");

            builder.HasIndex(x => new { x.SubmissionId, x.Index })
                .HasDatabaseName("uq_test_case_results_submission_id_test_case_index")
                .IsUnique();
        }
    }
}