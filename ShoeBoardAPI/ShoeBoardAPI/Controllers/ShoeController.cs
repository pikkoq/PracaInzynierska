using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.ShoeDtos;
using ShoeBoardAPI.Services.ShoeService;
using System.Security.Claims;

namespace ShoeBoardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoeController : ControllerBase
    {
        private readonly IShoeService _shoeService;

        public ShoeController(IShoeService shoeService)
        {
            _shoeService = shoeService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("RegisterNewShoe")]
        public async Task<IActionResult> RegisterNewShoe([FromBody] NewShoeRegistryDto newShoe)
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
            var result = await _shoeService.NewShoeRegistry(newShoe, userId);
            if (result.Success)
            {
                result.Message = "Added new shoe successfully!";
                return Ok(result);
            }

            return BadRequest(result);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("SignShoeToUser")]
        public async Task<IActionResult> SignShoeToUser([FromBody] SignShoeToUserDto newShoe, int? shoeCatalogId, int? userShoeCataloId)
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

            var result = await _shoeService.SignShoeToUser(newShoe, shoeCatalogId, userShoeCataloId, userId);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetAllUserShoes")]
        public async Task<IActionResult> GetAllUserShoes()
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

            var result = await _shoeService.GetAllUserShoes(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            result.Message = "Failed to fetch data";
            result.Success = false;
            result.Data = null;
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetAllUserShoesWithId")]
        public async Task<IActionResult> GetAllUserShoesWithId(string userId)
        {
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found. Cannot indetify."
                };
                return BadRequest(response);
            }

            var result = await _shoeService.GetAllUserShoes(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            result.Message = "Failed to fetch data";
            result.Success = false;
            result.Data = null;
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetAllAddedUserShoes")]
        public async Task<IActionResult> GetAllAddedUserShoes()
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

            var result = await _shoeService.GetAllAddedUserShoes(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            result.Message = "Failed to fetch data";
            result.Success = false;
            result.Data = null;
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetAllAddedUserShoesWithId")]
        public async Task<IActionResult> GetAllAddedUserShoesWithId(string userId)
        {
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found. Cannot indetify."
                };
                return BadRequest(response);
            }

            var result = await _shoeService.GetAllAddedUserShoes(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            result.Message = "Failed to fetch data";
            result.Success = false;
            result.Data = null;
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetShoeDetails")]
        public async Task<IActionResult> GetShoeDetails(int shoeId)
        {
            var response = await _shoeService.GetShoeDetails(shoeId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
