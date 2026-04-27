using Microsoft.EntityFrameworkCore;
using TaskForge.Domain.Common;

namespace TaskForge.Infrastructure.Persistence
{
    public class TaskForgeDbContext : DbContext
    {
        public TaskForgeDbContext(DbContextOptions<TaskForgeDbContext> options) : base(options)
        {
        }

        public DbSet<Domain.Entities.TaskItem> Tasks { get; set; }

        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

        public DbSet<TaskAuditLog> TaskAuditLogs => Set<TaskAuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.TaskItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Title).IsRequired().HasMaxLength(200);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken=default)
        {
            var domainEvents = ChangeTracker.Entries<BaseEntity>()
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            foreach(var domainEvent in domainEvents)
            {
                var outboxMessage = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOn = domainEvent.OccurredOn,
                    Type = domainEvent.GetType().FullName,
                    Payload = System.Text.Json.JsonSerializer.Serialize(domainEvent)
                };

                OutboxMessages.Add(outboxMessage);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
