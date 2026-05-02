using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TaskForge.Infrastructure.Persistence;
using TaskForge.Application.Common.Interfaces;
using TaskForge.Infrastructure.Communication;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
    var configuration = hostContext.Configuration;
    services.AddDbContext<TaskForgeDbContext>(options =>
    {
        options.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"));
    });
    // Possibly other service registrations here
    services.AddSingleton<ISmsService>(sp =>
        new TwilioSmsService(
            configuration["Twilio:AccountSid"],
            configuration["Twilio:AuthToken"],
            configuration["Twilio:FromNumber"]));
    })
    .Build()
    .Run();