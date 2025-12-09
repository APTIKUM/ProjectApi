using Microsoft.AspNetCore.Mvc;
using ProjectApi.DTOs;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KidsController : ControllerBase
    {
        private readonly IKidService _kidService;

        public KidsController(IKidService kidService)
        {
            _kidService = kidService;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<Kid>>> GetKids()
        //{
        //    var kids = await _kidService.GetAllKidsAsync();
        //    return Ok(kids);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Kid>> GetKid(string id)
        {
            var kid = await _kidService.GetKidByIdAsync(id);
            if (kid == null) return NotFound();
            return Ok(kid);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Kid>> UpdateKid(string id, KidUpdateDto kidUpdate)
        {
            var kidUpdated = await _kidService.UpdateKidAsync(id, kidUpdate);
            if (kidUpdated == null) return NotFound();
            return Ok(kidUpdated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKid(string id)
        {
            var result = await _kidService.DeleteKidAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{parentId}")]
        public async Task<ActionResult<Kid>> CreateKid(int parentId)
        {
            try
            {
                var createdKid = await _kidService.CreateKidAsync(parentId);
                return CreatedAtAction(
                    nameof(GetKid),
                    new { id = createdKid.Id },
                    createdKid
                );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<List<KidTask>>> GetKidTasks(string id)
        {
            var kidTasks = await _kidService.GetKidTasksAsync(id);
            return Ok(kidTasks);
        } 
    }
}