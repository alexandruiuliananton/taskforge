using TaskForge.Domain.Entities;

namespace TaskForge.Domain.Events
{
    public class TaskCreatedEvent : IDomainEvent
    {
        public Guid TaskId { get; private set; }
        public TaskPriority TaskPriority { get; private set; }
        public DateTime OccurredOn
        {
            get
            {
                return DateTime.Now;
            }
        }

        public TaskCreatedEvent(Guid taskId, TaskPriority taskPriority)
        {
            TaskId = taskId;
            TaskPriority = taskPriority;
        }

    }
}