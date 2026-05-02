using System.Text.Json;
using TaskForge.Application.Common.Constants;
using TaskForge.Application.Common.Interfaces;
using TaskForge.Application.DTOs;
using TaskForge.Domain.Entities;
using TaskForge.Domain.Events;

namespace TaskForge.Application.Commands.Tasks.CreateTask
{
    public class CreateTaskHandler
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IEventPublisher _eventPublisher;

        public CreateTaskHandler(ITaskRepository taskRepository, IEventPublisher eventPublisher)
        {
            _taskRepository = taskRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand command)
        {
            var task = new TaskItem(command.Title, command.Description, command.Priority);

            task.SetCorrelationId(command.CorrelationId);

            await _taskRepository.AddAsync(task);

            var eventDto = new TaskCreatedEventDto
            {
                TaskId = task.Id,
                OccurredOn = DateTime.UtcNow,
                CorrelationId = command.CorrelationId,
                Priority = task.Priority
            };

            var payload = JsonSerializer.Serialize(eventDto);

            await _eventPublisher.PublishAsync(EventTypes.TaskCreated, payload);

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status.ToString()
            };
        }
    }
}
