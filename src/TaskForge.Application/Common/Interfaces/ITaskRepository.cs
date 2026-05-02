using TaskForge.Domain.Entities;

namespace TaskForge.Application.Common.Interfaces
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem item);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task UpdateAsync(TaskItem item);
        Task DeleteAsync(Guid id);
    }
}