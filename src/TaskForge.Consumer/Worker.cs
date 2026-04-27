using Azure.Messaging.ServiceBus;
using System.Text.Json;
using TaskForge.Domain.Events;
using TaskForge.Infrastructure.Persistence;

namespace TaskForge.Consumer;

public class Worker: BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor _serviceBusProcessor;

    public Worker(IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _configuration = configuration;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var connection = _configuration["ServiceBus:ConnectionString"];
        var queueName = _configuration["ServiceBus:QueueName"];

        _serviceBusClient = new ServiceBusClient(connection);
        _serviceBusProcessor = _serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,
            MaxConcurrentCalls = 1
        });

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _serviceBusProcessor.ProcessMessageAsync += MessageHandler;
        _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

        await _serviceBusProcessor.StartProcessingAsync(stoppingToken);
    }

    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var json = args.Message.Body.ToString();

        var eventData = JsonSerializer.Deserialize<TaskCreatedEventDto>(json);

        using var scope = _serviceScopeFactory.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<TaskForgeDbContext>();

        var audit = new TaskAuditLog
        {
            Id = Guid.NewGuid(),
            EventType = args.Message.Subject ?? "TaskCreatedEvent",
            OccurredOn = eventData?.OccurredOn ?? DateTime.UtcNow,
            CorrelationId = eventData?.CorrelationId,
            UserId = eventData?.UserId,
            Payload = json,
            Source = "TaskForge.Consumer"
        };

        database.TaskAuditLogs.Add(audit);
        await database.SaveChangesAsync();

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());

        return Task.CompletedTask;
    }
}
