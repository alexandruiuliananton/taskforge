using Operis.Domain.Entities;

namespace Operis.Application.Common.Interfaces
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem item);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task UpdateAsync(TaskItem item);
        Task DeleteAsync(Guid id);
    }
}