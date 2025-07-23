using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Blazor.News.Data
{
    public class NewsDBContextFactory : IDesignTimeDbContextFactory<NewsDBContext>
    {
        public NewsDBContext CreateDbContext(string[] args)
        {
            // Build configuration from appsettings.json of the main application.
            // This assumes appsettings.json is in the directory where the ef command is run,
            // or in the output directory after build.
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("NewsDatabase");

            var builder = new DbContextOptionsBuilder<NewsDBContext>();
            builder.UseSqlServer(connectionString);

            return new NewsDBContext(builder.Options);
        }
    }
}