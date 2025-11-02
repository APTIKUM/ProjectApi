using Microsoft.AspNetCore.Mvc;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentsController(IParentService parentService)
        {
            _parentService = parentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parent>>> GetParents()
        {
            var parents = await _parentService.GetAllParentsAsync();
            return Ok(parents);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Parent>> GetParent(int id)
        {
            var parent = await _parentService.GetParentByIdAsync(id);

            if (parent == null)
            {
                return NotFound();
            }

            return Ok(parent);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Parent>> CreateParent(Parent parent)
        {
            var createdParent = await _parentService.CreateParentAsync(parent);

            return CreatedAtAction(nameof(GetParent), new { id = createdParent.Id }, createdParent);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Parent>> Login(string email, string password)
        {
            var parent = await _parentService.LoginAsync(email, password);

            if (parent == null)
            {
                return Unauthorized();
            }

            return Ok(parent);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateParent(int id, Parent parent)
        {
            var success = await _parentService.UpdateParentAsync(id, parent);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteParent(int id)
        {
            var success = await _parentService.DeleteParentAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        // ========== ОПЕРАЦИИ С ДЕТЬМИ РОДИТЕЛЯ ==========

        [HttpPost("{parentId}/create-kid")]
        public async Task<ActionResult<Kid>> CreateKidForParent(int parentId, Kid kid)
        {
            try
            {
                var createdKid = await _parentService.CreateKidForParentAsync(parentId, kid);
                return CreatedAtAction(
                    nameof(KidsController.GetKid),
                    "Kids",                        
                    new { id = createdKid.Id },  
                    createdKid       
        );
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Получить детей родителя
        [HttpGet("{parentId}/kids")]
        public async Task<ActionResult<List<Kid>>> GetParentKids(int parentId)
        {
            var kids = await _parentService.GetParentKidsAsync(parentId);
            return Ok(kids);
        }

        // Привязать существующего ребенка
        [HttpPost("{parentId}/add-kid/{kidId}")]
        public async Task<IActionResult> AddKidToParent(int parentId, string kidId)
        {
            var result = await _parentService.AddKidToParentAsync(parentId, kidId);
            if (!result) return NotFound();
            return Ok();
        }

        // Отвязать ребенка
        [HttpDelete("{parentId}/remove-kid/{kidId}")]
        public async Task<IActionResult> RemoveKidFromParent(int parentId, string kidId)
        {
            var result = await _parentService.RemoveKidFromParentAsync(parentId, kidId);
            if (!result) return NotFound();
            return NoContent();
        }

    }
}
