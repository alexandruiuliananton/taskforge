namespace TaskForge.Application.Commands.Tasks.UpdateTask;

public record UpdateTaskCommand(Guid Id, string Title, string? Description);