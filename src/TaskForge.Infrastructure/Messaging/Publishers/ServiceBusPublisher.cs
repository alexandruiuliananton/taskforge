using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace TaskForge.Infrastructure.Messaging.Publishers
{
    public class ServiceBusPublisher
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

        public async Task PublishTaskAsync(string messageType, string payLoad)
        {
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(payLoad))
            {
                Subject = messageType
            };

            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
