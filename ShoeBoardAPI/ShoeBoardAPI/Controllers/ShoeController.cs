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
    }
}
