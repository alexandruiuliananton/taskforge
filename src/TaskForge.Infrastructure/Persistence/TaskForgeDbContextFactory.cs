using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace TaskForge.Infrastructure.Persistence
{
    public class TaskForgeDbContextFactory : IDesignTimeDbContextFactory<TaskForgeDbContext>
    {
        public TaskForgeDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<TaskForgeDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new TaskForgeDbContext(optionsBuilder.Options);
        }
    }
}
