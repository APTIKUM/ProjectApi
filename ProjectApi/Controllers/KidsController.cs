using Microsoft.AspNetCore.Mvc;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;
using ProjectApi.Services.Implementations;

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

        [HttpGet]
        public async Task<ActionResult<List<Kid>>> GetKids()
        {
            var kids = await _kidService.GetAllKidsAsync();
            return Ok(kids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Kid>> GetKid(string id)
        {
            var kid = await _kidService.GetKidByIdAsync(id);
            if (kid == null) return NotFound();
            return Ok(kid);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKid(string id, Kid kid)
        {
            var result = await _kidService.UpdateKidAsync(id, kid);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKid(string id)
        {
            var result = await _kidService.DeleteKidAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPost("{parentId}")]
        public async Task<ActionResult<Kid>> CreateKid(int parentId, Kid kid)
        {
            try
            {
                var createdKid = await _kidService.CreateKidAsync(parentId, kid);
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
    }
}