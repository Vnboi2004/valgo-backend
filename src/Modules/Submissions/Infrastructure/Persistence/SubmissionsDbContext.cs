using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Domain.Aggregates;
using VAlgo.Modules.Submissions.Domain.Entities;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence
{
    public sealed class SubmissionsDbContext : DbContext
    {
        public DbSet<Submission> Submissions => Set<Submission>();
        public DbSet<TestCaseResult> TestCaseResults => Set<TestCaseResult>();

        public SubmissionsDbContext(DbContextOptions<SubmissionsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubmissionsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}