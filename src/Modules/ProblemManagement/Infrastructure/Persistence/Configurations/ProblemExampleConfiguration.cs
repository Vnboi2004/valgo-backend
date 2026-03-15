using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Configurations
{
    public sealed class ProblemExampleConfiguration : IEntityTypeConfiguration<ProblemExample>
    {
        public void Configure(EntityTypeBuilder<ProblemExample> builder)
        {
            builder.ToTable("problem_examples", schema: "problems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => ProblemExampleId.From(value));

            builder.Property<ProblemId>("problem_id")
                .HasColumnName("problem_id")
                .HasConversion(x => x.Value, value => ProblemId.From(value));

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .IsRequired();

            builder.Property(x => x.Input)
               .HasColumnName("input")
               .HasMaxLength(200)
               .IsRequired();

            builder.Property(x => x.Output)
                .HasColumnName("output")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Explanation)
                .HasColumnName("explanation")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.HasIndex("problem_id");
        }
    }
}