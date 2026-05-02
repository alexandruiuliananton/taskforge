using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskForge.Infrastructure.Messaging.Publishers;

namespace TaskForge.Infrastructure.Persistence
{
    public class OutboxProcessor : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ServiceBusPublisher _serviceBusPublisher;

        public OutboxProcessor(IServiceProvider serviceProvider, ServiceBusPublisher serviceBusPublisher)
        {
            _serviceProvider = serviceProvider;
            _serviceBusPublisher = serviceBusPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<TaskForgeDbContext>();

                var messages = await context.OutboxMessages.Where(x => x.ProcessedOn == null).ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    await _serviceBusPublisher.PublishAsync(message.Type, message.Payload);
                    message.ProcessedOn = DateTime.UtcNow;
                }

                await context.SaveChangesAsync(stoppingToken);

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
