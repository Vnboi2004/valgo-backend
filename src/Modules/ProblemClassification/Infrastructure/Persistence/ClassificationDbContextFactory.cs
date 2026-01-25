using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace VAlgo.Modules.ProblemClassification.Infrastructure.Persistence
{
    internal sealed class ClassificationDbContextFactory : IDesignTimeDbContextFactory<ClassificationDbContext>
    {
        public ClassificationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ClassificationDbContext>();

            optionsBuilder.UseNpgsql(configuration.GetConnectionString("ClassificationDb"));

            return new ClassificationDbContext(optionsBuilder.Options);
        }
    }
}