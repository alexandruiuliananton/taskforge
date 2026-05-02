using TaskForge.Domain.Entities;

namespace TaskForge.Application.Commands.Tasks.CreateTask
{
    public class CreateTaskCommand
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Normal;

        public string? CorrelationId { get; set; }
    }
}
