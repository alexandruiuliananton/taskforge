using Operis.Application.Common.Interfaces;

namespace Operis.Application.Commands.Tasks.DeleteTask;

public class DeleteTaskHandler
{
    private readonly ITaskRepository _taskRepository;

    public DeleteTaskHandler(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task Handle(DeleteTaskCommand command)
    {
        await _taskRepository.DeleteAsync(command.Id);
    }
}