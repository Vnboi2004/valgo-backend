using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Contests.Domain.Aggregates;
using VAlgo.Modules.Contests.Domain.Entities;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence
{
    public sealed class ContestsDbContext : DbContext
    {
        public DbSet<Contest> Contests => Set<Contest>();
        public DbSet<ContestProblem> ContestProblems => Set<ContestProblem>();
        public DbSet<ContestParticipant> ContestParticipants => Set<ContestParticipant>();

        public ContestsDbContext(DbContextOptions<ContestsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContestsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}