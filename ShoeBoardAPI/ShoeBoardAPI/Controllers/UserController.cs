using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.UserDtos;
using ShoeBoardAPI.Services.UserService;
using System.Security.Claims;

namespace ShoeBoardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        public UserController(IUserService userService, UserManager<User> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            var response = await _userService.ReigsterUser(registerUserDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            var response = await _userService.LoginUser(loginUserDto);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var response = await _userService.GetUser(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch("editUserData")]
        public async Task<IActionResult> EditUserData([FromBody] EditUserDto editUser)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found. Cannot indetify. Please re-login."
                };
                return BadRequest(response);
            }

            var result = await _userService.EditUserData(editUser, userId);
            if(result.Success){
                result.Message = "Profile updated successfully!";
                return Ok(result);
            }

            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch("changeUserPassword")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordDto changePassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found. Cannot indetify. Please re-login."
                };
                return BadRequest(response);
            }

            var result = await _userService.ChangeUserPassword(changePassword, userId);
            if (result.Success)
            {
                result.Message = "Password changed successfully!";
                return Ok(result);
            }

            result.Message = "Password change failed.";
            return BadRequest(result);
        }

    }
}
