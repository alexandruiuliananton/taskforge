using TaskForge.Application.Interfaces;

namespace TaskForge.Application.Commands.Tasks.UpdateTask;

public class UpdateTaskHandler
{
    private readonly ITaskRepository _taskRepository;

    public UpdateTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task Handle(UpdateTaskCommand command)
    {
        var task = await _taskRepository.GetByIdAsync(command.Id);
        if (task == null)
            throw new ArgumentException("Task not found");

        task.Title = command.Title;
        task.Description = command.Description;

        await _taskRepository.UpdateAsync(task);
    }
}