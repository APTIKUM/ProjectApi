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

        //[HttpGet]
        //public async Task<ActionResult<List<KidTask>>> GetKidTasks()
        //{
        //    var kidTasks = await _kidTaskService.GetAllKidTasksAsync();
        //    return Ok(kidTasks);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Kid>> GetKidTask(int id)
        {
            var kidTask = await _kidTaskService.GetTaskByIdAsync(id);
            if (kidTask == null) return NotFound();
            return Ok(kidTask);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKidTask(int id, KidTask kidTask)
        {
            var result = await _kidTaskService.UpdateTaskAsync(id, kidTask);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKidTask(int id)
        {
            var result = await _kidTaskService.DeleteTaskAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{kidId}")]
        public async Task<ActionResult<KidTask>> CreateKidTask(string kidId, KidTask kidTask)
        {
            try
            {
                var createdKidTask = await _kidTaskService.CreateTaskAsync(kidId, kidTask);
                return CreatedAtAction(
                    nameof(GetKidTask),
                    new { id = createdKidTask.Id },
                    createdKidTask
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}