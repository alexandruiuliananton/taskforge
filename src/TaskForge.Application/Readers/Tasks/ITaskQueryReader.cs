using TaskForge.Application.DTOs;

namespace TaskForge.Application.Readers.Tasks
{
    public interface ITaskQueryReader
    {
        Task<TaskDto?> GetById(Guid id);
        Task<List<TaskDto>> GetAll();
    }
}
