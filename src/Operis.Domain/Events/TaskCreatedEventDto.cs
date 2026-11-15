using Operis.Domain.Entities;

namespace Operis.Domain.Events
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
