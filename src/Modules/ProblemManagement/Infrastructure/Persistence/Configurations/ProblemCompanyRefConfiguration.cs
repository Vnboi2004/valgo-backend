using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Entities;
using VAlgo.Modules.ProblemManagement.Domain.ValueObjects;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence.Configurations
{
    public sealed class ProblemCompanyRefConfiguration : IEntityTypeConfiguration<ProblemCompanyRef>
    {
        public void Configure(EntityTypeBuilder<ProblemCompanyRef> builder)
        {
            builder.ToTable("problem_companies", schema: "problems");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .HasConversion(x => x.Value, value => ProblemCompanyRefId.From(value));

            builder.Property<ProblemId>("problem_id")
                .HasColumnName("problem_id")
                .HasConversion(x => x.Value, value => ProblemId.From(value));

            builder.Property(x => x.CompanyId)
                .HasColumnName("company_id")
                .IsRequired();

            // builder.HasOne<Problem>()
            //     .WithMany(p => p.Companies)
            //     .HasForeignKey("problem_id")
            //     .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex("problem_id");
        }
    }
}