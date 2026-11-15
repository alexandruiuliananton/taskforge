using Microsoft.EntityFrameworkCore;
using Operis.Application.DTOs;
using Operis.Application.Readers.Tasks;
using Operis.Infrastructure.Persistence;

namespace Operis.Infrastructure.Readers.Tasks
{
    public class TaskQueryReader : ITaskQueryReader
    {
        private readonly OperisDbContext _context;

        public TaskQueryReader(OperisDbContext context)
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
