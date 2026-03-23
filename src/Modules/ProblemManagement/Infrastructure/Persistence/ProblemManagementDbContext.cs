using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.ProblemManagement.Domain.Aggregates;
using VAlgo.Modules.ProblemManagement.Domain.Entities;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence
{
    public sealed class ProblemManagementDbContext : DbContext
    {
        public DbSet<Problem> Problems => Set<Problem>();
        public DbSet<TestCase> TestCases => Set<TestCase>();
        public DbSet<ProblemClassificationRef> ProblemClassificationRefs => Set<ProblemClassificationRef>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<ProblemExample> ProblemExamples => Set<ProblemExample>();
        public DbSet<ProblemHint> ProblemHints => Set<ProblemHint>();
        public DbSet<SimilarProblemRef> SimilarProblemRefs => Set<SimilarProblemRef>();
        public DbSet<ProblemCompanyRef> ProblemCompanyRefs => Set<ProblemCompanyRef>();
        public DbSet<ProblemCodeTemplate> ProblemCodeTemplates => Set<ProblemCodeTemplate>();

        public ProblemManagementDbContext(DbContextOptions<ProblemManagementDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProblemManagementDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}