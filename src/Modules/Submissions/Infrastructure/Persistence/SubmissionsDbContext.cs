using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Submissions.Application.Abstractions;
using VAlgo.Modules.Submissions.Domain.Aggregates;
using VAlgo.Modules.Submissions.Infrastructure.Persistence.Entities;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence
{
    public sealed class SubmissionsDbContext : DbContext, IUnitOfWork
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

        Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
            => base.SaveChangesAsync(cancellationToken);
    }
}