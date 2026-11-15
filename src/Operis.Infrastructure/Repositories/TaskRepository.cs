using Microsoft.EntityFrameworkCore;
using Operis.Application.Common.Interfaces;
using Operis.Domain.Entities;
using Operis.Infrastructure.Persistence;

namespace Operis.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly OperisDbContext _context;

        public TaskRepository(OperisDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItem item)
        {
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(TaskItem item)
        {
            _context.Tasks.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
