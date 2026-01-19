using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace VAlgo.Modules.Submissions.Infrastructure.Persistence
{
    internal sealed class SubmissionsDbContextFactory : IDesignTimeDbContextFactory<SubmissionsDbContext>
    {
        public SubmissionsDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsetiings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SubmissionsDbContext>();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("SubmissionsDb"));

            return new SubmissionsDbContext(optionsBuilder.Options);
        }
    }
}