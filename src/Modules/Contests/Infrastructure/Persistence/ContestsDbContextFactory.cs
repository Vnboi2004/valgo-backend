using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VAlgo.Modules.Contests.Infrastructure.Persistence
{
    internal sealed class ContestsDbContextFactory : IDesignTimeDbContextFactory<ContestsDbContext>
    {
        public ContestsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionBuilder = new DbContextOptionsBuilder<ContestsDbContext>();

            optionBuilder.UseNpgsql(configuration.GetConnectionString("ContestsDb"));

            return new ContestsDbContext(optionBuilder.Options);
        }
    }
}