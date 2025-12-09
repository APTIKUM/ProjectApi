using Microsoft.AspNetCore.Mvc;
using ProjectApi.DTOs;
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
        public async Task<ActionResult<KidTask>> UpdateKidTask(int id, KidTaskUpdateDto kidTask)
        {
            var updatedKidTask = await _kidTaskService.UpdateTaskAsync(id, kidTask);
            if (updatedKidTask == null) return NotFound();
            return Ok(updatedKidTask);
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult<KidTask>> ChangeCompletedAsync(int id, bool isSetCompleted, DateOnly? dateOnly = null)
        {
            var updatedKidTask = await _kidTaskService.ChangeCompletedAsync(id, isSetCompleted, dateOnly);
            if (updatedKidTask == null) return NotFound();
            return Ok(updatedKidTask);
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