namespace TaskForge.Infrastructure.Persistence
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Payload { get; set; }
        public DateTime? ProcessedOn { get; set; }
        public DateTime OccurredOn { get; internal set; }
    }
}
