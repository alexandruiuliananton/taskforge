using TaskForge.Domain.Common;
using TaskForge.Domain.Events;

namespace TaskForge.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public Guid Id { get; private set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; private set; }
        public string? CorrelationId { get; private set; }
        public TaskPriority Priority { get; private set; } = TaskPriority.Normal;

        public TaskItem(string title, string? description = null, TaskPriority priority = TaskPriority.Normal)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Status = TaskStatus.ToDo;
            Priority = priority;  // Changed from taskPriority to priority

            AddDomainEvent(new TaskCreatedEvent(Id, priority));  // Changed from taskPriority to priority
        }

        public void MarkInProgress()
        {
            if (Status != TaskStatus.ToDo)
                throw new InvalidOperationException("Invalid transition");

            Status = TaskStatus.InProgress;
        }

        public void MarkDone()
        {
            if (Status != TaskStatus.InProgress)
                throw new InvalidOperationException("Invalid transition");

            Status = TaskStatus.Done;
        }

        public void SetCorrelationId(string? correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
