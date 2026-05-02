using Microsoft.EntityFrameworkCore;
using TaskForge.Application.Common.Interfaces;
using TaskForge.Domain.Entities;
using TaskForge.Infrastructure.Persistence;

namespace TaskForge.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskForgeDbContext _context;

        public TaskRepository(TaskForgeDbContext context)
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
