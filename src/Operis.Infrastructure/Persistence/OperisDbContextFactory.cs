using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Operis.Infrastructure.Persistence
{
    public class OperisDbContextFactory : IDesignTimeDbContextFactory<OperisDbContext>
    {
        public OperisDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OperisDbContext>();
            optionsBuilder.UseSqlServer("");
            return new OperisDbContext(optionsBuilder.Options);
        }
    }
}
