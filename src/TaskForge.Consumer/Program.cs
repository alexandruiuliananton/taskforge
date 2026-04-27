using Microsoft.EntityFrameworkCore;
using TaskForge.Consumer;
using TaskForge.Infrastructure.Persistence;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();

        services.AddDbContext<TaskForgeDbContext>(options =>
        {
            options.UseSqlServer(
                context.Configuration.GetConnectionString("DefaultConnection"));
        });
    })
    .Build()
    .Run();