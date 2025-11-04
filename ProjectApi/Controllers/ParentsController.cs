using Microsoft.AspNetCore.Mvc;
using ProjectApi.Models;
using ProjectApi.Services.Abstractions;

namespace ProjectApi.Controllers
{
    [Route("api/parents")]
    [ApiController]
    public class ParentsController : ControllerBase
    {
        private readonly IParentService _parentService;

        public ParentsController(IParentService parentService)
        {
            _parentService = parentService;
        }

        // ========== ОСНОВНЫЕ ОПЕРАЦИИ С РОДИТЕЛЯМИ ==========

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parent>>> Get()
        {
            var parents = await _parentService.GetAllParentsAsync();
            return Ok(parents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Parent>> Get(int id)
        {
            var parent = await _parentService.GetParentByIdAsync(id);
            if (parent == null) return NotFound();
            return Ok(parent);
        }

        [HttpPost]
        public async Task<ActionResult<Parent>> Create(Parent parent)
        {
            var createdParent = await _parentService.CreateParentAsync(parent);
            return CreatedAtAction(nameof(Get), new { id = createdParent.Id }, createdParent);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Parent>> Login(LoginRequest request)
        {
            var parent = await _parentService.LoginAsync(request.Email, request.Password);
            if (parent == null) return Unauthorized();
            return Ok(parent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Parent parent)
        {
            var success = await _parentService.UpdateParentAsync(id, parent);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _parentService.DeleteParentAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        // ========== ОПЕРАЦИИ С ДЕТЬМИ РОДИТЕЛЯ ==========

        [HttpGet("{parentId}/kids")]
        public async Task<ActionResult<List<Kid>>> GetKids(int parentId)
        {
            var kids = await _parentService.GetParentKidsAsync(parentId);
            return Ok(kids);
        }

        [HttpPost("{parentId}/kids")]
        public async Task<ActionResult<Kid>> CreateKid(int parentId, Kid kid)
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

        [HttpPut("{parentId}/kids/{kidId}")]
        public async Task<IActionResult> AddKid(int parentId, string kidId)
        {
            var result = await _parentService.AddKidToParentAsync(parentId, kidId);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpDelete("{parentId}/kids/{kidId}")]
        public async Task<IActionResult> RemoveKid(int parentId, string kidId)
        {
            var result = await _parentService.RemoveKidFromParentAsync(parentId, kidId);
            if (!result) return NotFound();
            return NoContent();
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}