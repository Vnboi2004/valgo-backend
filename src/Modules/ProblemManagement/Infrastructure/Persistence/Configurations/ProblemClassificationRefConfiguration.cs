using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Configurations
{
    public sealed class ProblemClassificationRefConfiguration : IEntityTypeConfiguration<ProblemClassificationRef>
    {
        public void Configure(EntityTypeBuilder<ProblemClassificationRef> builder)
        {
            builder.ToTable("problem_classifications", schema: "problem_classifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(id => id.Value, value => ProblemClassificationRefId.From(value))
                .ValueGeneratedNever();

            builder.Property(x => x.ClassificationId)
                .HasColumnName("classification_id")
                .IsRequired();

            builder.HasOne<Problem>()
                .WithMany(p => p.Classifications)
                .HasForeignKey("problem_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}