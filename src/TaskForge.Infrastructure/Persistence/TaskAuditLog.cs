namespace TaskForge.Infrastructure.Persistence
{
    public class TaskAuditLog
    {
        public Guid Id { get; set; }

        public string EventType { get; set; } = default!;

        public DateTime OccurredOn { get; set; }

        public string? UserId { get; set; }

        public string? CorrelationId { get; set; }

        public string Payload { get; set; } = default!;

        public string Source { get; set; } = default!;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
