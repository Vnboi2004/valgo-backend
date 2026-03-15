using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Configurations
{
    public sealed class ProblemHintConfiguration : IEntityTypeConfiguration<ProblemHint>
    {
        public void Configure(EntityTypeBuilder<ProblemHint> builder)
        {
            builder.ToTable("problem_hints", schema: "problems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => ProblemHintId.From(value));

            builder.Property<ProblemId>("problem_id")
                .HasColumnName("problem_id")
                .HasConversion(x => x.Value, value => ProblemId.From(value));

            builder.Property(x => x.Order)
                .HasColumnName("order")
                .IsRequired();

            builder.Property(x => x.Content)
                .HasColumnName("content")
                .HasMaxLength(200)
                .IsRequired();

            builder.HasIndex("problem_id");
        }
    }
}