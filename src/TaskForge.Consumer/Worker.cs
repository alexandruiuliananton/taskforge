using Azure.Messaging.ServiceBus;
using System.Text.Json;
using TaskForge.Application.Common.Constants;
using TaskForge.Application.Common.Interfaces;
using TaskForge.Application.DTOs;
using TaskForge.Domain.Events;
using TaskForge.Infrastructure.Persistence;

namespace TaskForge.Consumer;

public class Worker : BackgroundService
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
        var subject = args.Message.Subject;

        var eventData = JsonSerializer.Deserialize<TaskCreatedEventDto>(json);

        using var scope = _serviceScopeFactory.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<TaskForgeDbContext>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

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

        switch (subject)
        {
            case EventTypes.TaskCreated:
                {
                    var task = JsonSerializer.Deserialize<TaskCreatedEventDto>(json);

                    if (task == null) break;

                    audit.CorrelationId = task.CorrelationId;
                    await database.SaveChangesAsync();

                    if (task.Priority == Domain.Entities.TaskPriority.High)
                    {
                        var message = new MessageCreatedEventDto
                        {
                            To = "+40758218273",
                            Body = $"A new high priority task has been created: {task.TaskId}",
                            CorrelationId = task.CorrelationId
                        };

                        await eventPublisher.PublishAsync(EventTypes.MessageCreated, JsonSerializer.Serialize(message));
                    }

                    break;
                }

            case EventTypes.MessageCreated:
                {
                    var message = JsonSerializer.Deserialize<MessageCreatedEventDto>(json);
                    if (message == null) break;

                    var smsService = scope.ServiceProvider.GetRequiredService<ISmsService>();

                    await SendSmsWithRetryAsync(smsService, message.To, message.Body, message.CorrelationId);

                    break;
                }

            default:
                Console.WriteLine($"Unknown message subject: {subject}");
                break;
        }

        await args.CompleteMessageAsync(args.Message);
    }

    private Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());

        return Task.CompletedTask;
    }

    private async Task SendSmsWithRetryAsync(
        ISmsService smsService,
        string to,
        string body,
        string correlationId)

    {
        int maxRetries = 3;
        int attempt = 0;
        bool success = false;

        while (attempt < maxRetries && !success)
        {
            try
            {
                attempt++;

                Console.WriteLine("$[SMS] Attempt {attempt} | Correlation={correlationId}");

                await smsService.SendSmsAsync(to, body);

                success = true;

                Console.WriteLine($"[SMS] SUCCESS | CorrelationId={correlationId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SMS] ERROR attempt {attempt} | CorrelationId={correlationId} | {ex.Message}");

                if (attempt >= maxRetries)
                {
                    Console.WriteLine($"[SMS] FAILED after {attempt} attempts | CorrelationId={correlationId}");
                    throw;
                }

                await Task.Delay(1000 * attempt); // Wait before retrying
            }
        }
    }
}
