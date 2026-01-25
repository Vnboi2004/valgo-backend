using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemClassification.Domain.Aggregates;

namespace VAlgo.Modules.ProblemClassification.Infrastructure.Persistence
{
    public sealed class ClassificationDbContext : DbContext
    {
        public DbSet<Classification> Classifications => Set<Classification>();

        public ClassificationDbContext(DbContextOptions<ClassificationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClassificationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}