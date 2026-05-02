using TaskForge.Domain.Entities;

namespace TaskForge.Domain.Events
{
    public class TaskCreatedEventDto
    {
        public Guid TaskId { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime OccurredOn { get; set; }

        public string? CorrelationId { get; set; }

        public string? UserId { get; set; }
    }
}
