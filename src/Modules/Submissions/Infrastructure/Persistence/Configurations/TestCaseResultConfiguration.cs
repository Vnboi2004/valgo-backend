using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.Submissions.Infrastructure.Persistence.Entities;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence.Configurations
{
    public sealed class TestCaseResultConfiguration : IEntityTypeConfiguration<TestCaseResult>
    {
        public void Configure(EntityTypeBuilder<TestCaseResult> builder)
        {
            builder.ToTable("TestCaseResults", schema: "testCaseResults");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.SubmissionId)
                .IsRequired();

            builder.Property(x => x.Index)
                .IsRequired();

            builder.Property(x => x.Passed)
                .IsRequired();

            builder.Property(x => x.TimeMs)
                .IsRequired();

            builder.Property(x => x.MemoryKb)
                .IsRequired();

            builder.Property(x => x.Error)
                .HasMaxLength(200);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasIndex(x => x.SubmissionId);

            builder.HasIndex(x => new { x.SubmissionId, x.Index })
                .IsUnique();
        }
    }
}