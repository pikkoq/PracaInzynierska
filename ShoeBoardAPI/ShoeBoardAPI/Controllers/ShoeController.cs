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
        public async Task<IActionResult> GetAllUserShoes(string userId)
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
        public async Task<IActionResult> GetAllAddedUserShoes(string userId)
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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("SearchShoe")]
        public async Task<IActionResult> SearchShoes(string searchTerm, int pageNumber = 1)
        {
            var response = await _shoeService.SearchShoes(searchTerm, pageNumber);
            return response.Success ? Ok(response) : BadRequest(response.Success = false);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("SearchUsersShoe")]
        public async Task<IActionResult> SearchUsersShoes(string searchTerm, int pageNumber = 1)
        {
            var response = await _shoeService.SearchUsersShoes(searchTerm, pageNumber);
            return response.Success ? Ok(response) : BadRequest(response.Success = false);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch("EditUserAddedShoe")]
        public async Task<IActionResult> EditUserAddedShoe(int shoeId,[FromBody] EditShoeDetailsDto updatedShoe)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _shoeService.EditUserAddedShoeDetails(shoeId, updatedShoe, userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch("EditUserShoe")]
        public async Task<IActionResult> EditUserShoe(int shoeId, [FromBody] EditUserShoeDto updatedShoe)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _shoeService.EditUserShoe(shoeId, updatedShoe, userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeleteUserShoe")]
        public async Task<IActionResult> DeleteUserShoe(int shoeId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _shoeService.DeleteUserShoe(shoeId, userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetPopularShoes")]
        public async Task<IActionResult> GetPopularShoes()
        {

            var response = await _shoeService.GetTopPoulrShoes();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetCatalogShoeDetails")]
        public async Task<IActionResult> GetCatalogShoeDetails(int catalogShoeId)
        {

            var response = await _shoeService.GetCatalogShoeDetails(catalogShoeId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
