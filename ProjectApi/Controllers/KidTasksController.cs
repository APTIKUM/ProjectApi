using Microsoft.AspNetCore.Mvc;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KidTasksController : ControllerBase
    {
        private readonly IKidTaskService _kidTaskService;

        public KidTasksController(IKidTaskService kidTaskService)
        {
            _kidTaskService = kidTaskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Kid>>> GetKidTasks()
        {
            var kids = await _kidTaskService.GetAllKidTasksAsync();
            return Ok(kids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kid>> GetKidTask(int id)
        {
            var kid = await _kidTaskService.GetTaskByIdAsync(id);
            if (kid == null) return NotFound();
            return Ok(kid);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKid(int id, KidTask kidTask)
        {
            var result = await _kidTaskService.UpdateTaskAsync(id, kidTask);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKid(int id)
        {
            var result = await _kidTaskService.DeleteTaskAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}