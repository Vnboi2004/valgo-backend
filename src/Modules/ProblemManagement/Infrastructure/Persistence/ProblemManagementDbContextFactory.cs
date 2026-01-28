using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VAlgo.Modules.ProblemManagement.Infractructure.Persistence
{
    internal sealed class ProblemManagementDbContextFactory : IDesignTimeDbContextFactory<ProblemManagementDbContext>
    {
        public ProblemManagementDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ProblemManagementDbContext>();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("ProblemManagementDb"));

            return new ProblemManagementDbContext(optionsBuilder.Options);
        }
    }
}