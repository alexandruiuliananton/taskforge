using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text;
using TaskForge.Application.Common.Interfaces;

namespace TaskForge.Infrastructure.Messaging.Publishers
{
    public class ServiceBusPublisher : IEventPublisher
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _serviceBusSender;

        public ServiceBusPublisher(IConfiguration configuration)
        {
            var connectionString = configuration["ServiceBus:ConnectionString"];
            var queueName = configuration["ServiceBus:QueueName"];

            _serviceBusClient = new ServiceBusClient(connectionString);
            _serviceBusSender = _serviceBusClient.CreateSender(queueName);
        }

        public async Task PublishAsync(string messageType, string payload)
        {
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(payload))
            {
                Subject = messageType
            };

            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
