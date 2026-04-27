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

        public TaskItem(string title, string? description = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            Status = TaskStatus.ToDo;

            AddDomainEvent(new TaskCreatedEvent(Id));
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
