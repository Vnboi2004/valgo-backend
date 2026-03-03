using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Configurations
{
    public sealed class TestCaseConfiguration : IEntityTypeConfiguration<TestCase>
    {
        public void Configure(EntityTypeBuilder<TestCase> builder)
        {
            builder.ToTable("problem_test_cases", schema: "problems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => TestCaseId.From(value))
                .ValueGeneratedNever();

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .IsRequired();

            builder.Property(x => x.Input)
                .HasColumnName("input")
                .IsRequired();

            builder.Property(x => x.ExpectedOutput)
                .HasColumnName("expected_output")
                .IsRequired();

            builder.Property(x => x.ComparisonStrategy)
                .HasColumnName("output_comparion_strategy")
                .HasConversion<int>()
                .IsRequired();

            builder.Property(x => x.IsSample)
                .HasColumnName("is_sample")
                .IsRequired();

            builder.HasOne<Problem>()
                .WithMany(p => p.TestCases)
                .HasForeignKey("problem_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.Order });
        }
    }
}