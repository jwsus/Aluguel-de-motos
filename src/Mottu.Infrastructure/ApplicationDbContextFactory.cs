using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Mottu.Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Set the base path to the directory where the factory is being run from
            var basePath = Directory.GetCurrentDirectory();
            
            // Build the configuration to read the appsettings.json file
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            // Create the options builder and configure it to use Npgsql with the connection string from the configuration
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);

            // Return a new instance of ApplicationDbContext with the configured options
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
