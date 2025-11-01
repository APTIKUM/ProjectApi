using Microsoft.AspNetCore.Http;
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


        [HttpGet("{id:int}")]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParent(int id, Parent parent)
        {
            var success = await _parentService.UpdateParentAsync(id, parent);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParent(int id)
        {
            var success = await _parentService.DeleteParentAsync(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
