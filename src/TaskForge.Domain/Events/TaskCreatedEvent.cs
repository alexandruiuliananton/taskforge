namespace TaskForge.Domain.Events
{
    public class TaskCreatedEvent : IDomainEvent
    {
        public Guid Id { get; set; }
        public DateTime OccurredOn
        {
            get
            {
                return DateTime.Now;
            }
        }

        public TaskCreatedEvent(Guid taskId)
        {
            taskId = taskId;
        }

    }
}