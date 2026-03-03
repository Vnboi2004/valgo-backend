using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Domain.Aggregates;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence
{
    public sealed class SubmissionsDbContext : DbContext
    {
        public DbSet<Submission> Submissions => Set<Submission>();

        public SubmissionsDbContext(DbContextOptions<SubmissionsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubmissionsDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}