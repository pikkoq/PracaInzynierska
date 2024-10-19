using Microsoft.AspNetCore.Mvc;
using ShoeBoardAPI.Models.DTO.UserDtos;
using ShoeBoardAPI.Services.UserService;

namespace ShoeBoardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto registerUserDto)
        {
            var response = await _userService.ReigsterUser(registerUserDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUser(LoginUserDto loginUserDto)
        {
            var response = await _userService.LoginUser(loginUserDto);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

    }
}
