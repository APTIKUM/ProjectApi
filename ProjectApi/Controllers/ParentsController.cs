using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectApi.DTOs;
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

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Parent>>> GetParents()
        //{
        //    var parents = await _parentService.GetAllParentsAsync();
        //    return Ok(parents);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<Parent>> GetParent(int id)
        {
            var parent = await _parentService.GetParentByIdAsync(id);
            if (parent == null) return NotFound();
            return Ok(parent);
        }

        [HttpPost]
        public async Task<ActionResult<Parent>> Register(ParentRegisterDto parent)
        {
            var createdParent = await _parentService.RegisterParentAsync(parent);
            return CreatedAtAction(nameof(GetParent), new { id = createdParent.Id }, createdParent);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Parent>> Login(string email, string password)
        {
            var parent = await _parentService.LoginAsync(email, password);
            if (parent == null) return Unauthorized();

            return Ok(parent);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Parent>> Update(int id, ParentUpdateDto parentUpdate)
        {
            var parentUpdated = await _parentService.UpdateParentAsync(id, parentUpdate);
            if (parentUpdated == null) return NotFound();

            return Ok(parentUpdated);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> ChangePassword(int id, string currentPassword, string newPassword)
        {
            var success = await _parentService.ChangePassword(id, currentPassword, newPassword);
            if (!success) return BadRequest();

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _parentService.DeleteParentAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }


        [HttpGet("{parentId}/kids")]
        public async Task<ActionResult<List<Kid>>> GetKids(int parentId)
        {
            var kids = await _parentService.GetParentKidsAsync(parentId);
            return Ok(kids);
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
}