namespace TaskForge.Domain.Events
{
    public class TaskCreatedEventDto
    {
        public DateTime OccurredOn { get; set; }

        public string? CorrelationId { get; set; }

        public string? UserId { get; set; }
    }
}
