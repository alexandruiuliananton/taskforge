using TaskForge.Application.Common.Interfaces;

namespace TaskForge.Application.Commands.Tasks.CompleteTask;

public class CompleteTaskHandler
{
    private readonly ITaskRepository _taskRepository;

    public CompleteTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task Handle(CompleteTaskCommand command)
    {
        var task = await _taskRepository.GetByIdAsync(command.Id);
        if (task == null)
            throw new ArgumentException("Task not found");

        if (task.Status != Domain.Entities.TaskStatus.InProgress) { task.MarkInProgress(); }

        task.MarkDone();

        await _taskRepository.UpdateAsync(task);
    }
}