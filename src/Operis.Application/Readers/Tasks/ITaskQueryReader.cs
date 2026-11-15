using Operis.Application.DTOs;

namespace Operis.Application.Readers.Tasks
{
    public interface ITaskQueryReader
    {
        Task<TaskDto?> GetById(Guid id);
        Task<List<TaskDto>> GetAll();
    }
}
