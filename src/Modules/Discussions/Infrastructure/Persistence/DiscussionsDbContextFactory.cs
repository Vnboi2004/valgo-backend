using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VAlgo.Modules.Discussions.Infrastructure.Persistence
{
    internal sealed class DiscussionsDbContextFactory : IDesignTimeDbContextFactory<DiscussionsDbContext>
    {
        public DiscussionsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DiscussionsDbContext>();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DiscussionsDb"));

            return new DiscussionsDbContext(optionsBuilder.Options);
        }
    }
}