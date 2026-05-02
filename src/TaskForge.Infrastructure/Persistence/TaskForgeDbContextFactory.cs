using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskForge.Infrastructure.Persistence
{
    public class TaskForgeDbContextFactory : IDesignTimeDbContextFactory<TaskForgeDbContext>
    {
        public TaskForgeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TaskForgeDbContext>();
            optionsBuilder.UseSqlServer("");
            return new TaskForgeDbContext(optionsBuilder.Options);
        }
    }
}
