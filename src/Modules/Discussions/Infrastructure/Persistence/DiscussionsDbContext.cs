using Microsoft.EntityFrameworkCore;
using VAlgo.Modules.Discussions.Domain.Aggregates;
using VAlgo.Modules.Discussions.Domain.Entities;

namespace VAlgo.Modules.Discussions.Infrastructure.Persistence
{
    public sealed class DiscussionsDbContext : DbContext
    {
        public DbSet<Discussion> Discussions => Set<Discussion>();
        public DbSet<Comment> Comments => Set<Comment>();

        public DiscussionsDbContext(DbContextOptions<DiscussionsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscussionsDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}