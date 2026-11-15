using Microsoft.AspNetCore.Mvc;
using Operis.Application.Commands.Tasks.CompleteTask;
using Operis.Application.Commands.Tasks.CreateTask;
using Operis.Application.Commands.Tasks.DeleteTask;
using Operis.Application.Commands.Tasks.UpdateTask;
using Operis.Application.Readers.Tasks;

namespace Operis.Api.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly CreateTaskHandler _createHandler;
        private readonly UpdateTaskHandler _updateHandler;
        private readonly CompleteTaskHandler _completeHandler;
        private readonly DeleteTaskHandler _deleteHandler;
        private readonly ITaskQueryReader _query;

        public TasksController(
            CreateTaskHandler createHandler,
            UpdateTaskHandler updateHandler,
            CompleteTaskHandler completeHandler,
            DeleteTaskHandler deleteHandler,
            ITaskQueryReader query)
        {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _completeHandler = completeHandler;
            _deleteHandler = deleteHandler;
            _query = query;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskCommand command)
        {
            var correlationId = HttpContext.Items["CorrelationId"]?.ToString();

            command.CorrelationId = correlationId;

            var id = await _createHandler.Handle(command);

            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateTaskCommand command)
        {
            if (id != command.Id) return BadRequest();

            await _updateHandler.Handle(command);

            return NoContent();
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _completeHandler.Handle(new CompleteTaskCommand(id));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deleteHandler.Handle(new DeleteTaskCommand(id));

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var task = await _query.GetById(id);

            return task == null ? NotFound() : Ok(task);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _query.GetAll();

            return Ok(tasks);
        }
    }
}
