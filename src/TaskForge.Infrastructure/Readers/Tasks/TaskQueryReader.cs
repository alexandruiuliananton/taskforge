using Microsoft.EntityFrameworkCore;
using TaskForge.Application.DTOs;
using TaskForge.Application.Readers.Tasks;
using TaskForge.Infrastructure.Persistence;

namespace TaskForge.Infrastructure.Readers.Tasks
{
    public class TaskQueryReader : ITaskQueryReader
    {
        private readonly TaskForgeDbContext _context;

        public TaskQueryReader(TaskForgeDbContext context)
        {
            _context = context;
        }

        public async Task<TaskDto?> GetById(Guid id)
        {
            return await _context.Tasks
                .Where(t => t.Id == id)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status.ToString()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<TaskDto>> GetAll()
        {
            return await _context.Tasks
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status.ToString()
                })
                .ToListAsync();
        }
    }
}
