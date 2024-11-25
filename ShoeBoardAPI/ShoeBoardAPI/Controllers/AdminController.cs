using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeBoardAPI.Models.DTO.AdminDtos;
using ShoeBoardAPI.Services.AdminService.cs;

namespace ShoeBoardAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("GetShoesToAccept")]
        public async Task<IActionResult> GetShoesToAccept([FromQuery]int pageNumber = 1)
        {
            var response = await _adminService.GetShoesToAccept(pageNumber);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("AcceptNewAddedShoes")]
        public async Task<IActionResult> AcceptNewAddedShoes(int shoeId)
        {
            var response = await _adminService.AccecptNewAddedShoes(shoeId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("DeclineNewAddedShoes")]
        public async Task<IActionResult> DeclineNewAddedShoes(int shoeId)
        {
            var response = await _adminService.DeclineNewAddedShoes(shoeId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("EditNewAddedShoes")]
        public async Task<IActionResult> EditNewAddedShoes(int shoeId, [FromBody] EditNewAddedShoesDto editShoeDto)
        {
            var response = await _adminService.EditNewAddedShoes(shoeId, editShoeDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1)
        {
            var response = await _adminService.GetAllUsers(pageNumber);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("DeleteUserAccount")]
        public async Task<IActionResult> DeleteUserAccount(string userId)
        {
            var response = await _adminService.DeleteUserAccount(userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("EditUserAccount")]
        public async Task<IActionResult> EditUserAccount(string userId, [FromBody] EditUserAccountDto editUserDto)
        {
            var response = await _adminService.EditUserAccount(userId, editUserDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAllUsersPosts")]
        public async Task<IActionResult> GetAllUsersPosts([FromQuery] int pageNumber = 1)
        {
            var response = await _adminService.GetAllUsersPosts(pageNumber);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("EditUserPost")]
        public async Task<IActionResult> EditUserPost(int postId, [FromBody] string content)
        {
            var response = await _adminService.EditUserPost(postId, content);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("DeleteUserPost")]
        public async Task<IActionResult> DeleteUserPost(int postId)
        {
            var response = await _adminService.DeleteUserPost(postId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("DeleteUserComment")]
        public async Task<IActionResult> DeleteUserComment(int commentId)
        {
            var response = await _adminService.DeleteComment(commentId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("EditUserComment")]
        public async Task<IActionResult> EditUserComment(int commentId, [FromBody] string content)
        {
            var response = await _adminService.EditComment(commentId, content);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
