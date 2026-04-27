using TaskForge.Application.DTOs;
using TaskForge.Application.Interfaces;
using TaskForge.Domain.Entities;

namespace TaskForge.Application.Commands.Tasks.CreateTask
{
    public class CreateTaskHandler
    {
        private readonly ITaskRepository _taskRepository;

        public CreateTaskHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand command)
        {
            var task = new TaskItem(command.Title);

            task.SetCorrelationId(command.CorrelationId);

            await _taskRepository.AddAsync(task);

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status.ToString()
            };
        }
    }
}
