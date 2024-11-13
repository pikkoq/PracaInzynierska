using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeBoardAPI.Models.DTO.ShoeDtos;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Services.FriendService;
using System.Security.Claims;
using Azure.Core;

namespace ShoeBoardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;

        public FriendController(IFriendService friendService)
        {
            _friendService = friendService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("SendFriendRequest")]
        public async Task<IActionResult> SendFriendRequest(string receiverId)
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
            var result = await _friendService.SendFriendRequest(userId, receiverId);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetFriendRequests")]
        public async Task<IActionResult> GetFriendRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "User not found. Cannot indetify."
                };
                return BadRequest(response);
            }

            var result = await _friendService.GetFriendRequests(userId);
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
        [HttpGet("GetSentFriendRequests")]
        public async Task<IActionResult> GetSentFriendRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "User not found. Cannot indetify."
                };
                return BadRequest(response);
            }

            var result = await _friendService.GetSentFriendRequests(userId);
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
        [HttpGet("GetFreinds")]
        public async Task<IActionResult> GetFriends()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "User not found. Cannot indetify."
                };
                return BadRequest(response);
            }

            var result = await _friendService.GetFriends(userId);
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
        [HttpGet("SearchFriends")]
        public async Task<IActionResult> SearchFriends(string searchTerm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "User not found. Cannot indetify."
                };
                return BadRequest(response);
            }

            var result = await _friendService.SearchFriends(searchTerm, userId);
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
        [HttpPost("AcceptFriendRequest")]
        public async Task<IActionResult> AcceptFriendRequest(int requestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "User not found. Cannot indetify."
                };
                return BadRequest(response);
            }

            var result = await _friendService.AcceptFriendRequest(requestId, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeleteFriend")]
        public async Task<IActionResult> DeleteFriend(string friendId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new ServiceResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "User not found. Cannot indetify."
            };

            var result = await _friendService.DeleteFriend(userId, friendId);
            if (result.Success)
            {
                return Ok(result);
            }
            result.Message = "Failed to delete friend.";
            result.Success = false;
            result.Data = false;
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeclineFriendRequest")]
        public async Task<IActionResult> DeclineFriendRequest(int requestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new ServiceResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "User not found. Cannot indetify."
            };

            var result = await _friendService.DeclineFriendRequest(requestId, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            result.Message = "Failed to decline friend request.";
            result.Success = false;
            result.Data = false;
            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("CancelFriendRequest")]
        public async Task<IActionResult> CancelFriendRequest(int requestId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = new ServiceResponse<bool>
            {
                Data = false,
                Success = false,
                Message = "User not found. Cannot indetify."
            };

            var result = await _friendService.CancelSentFriendRequest(requestId, userId);
            if (result.Success)
            {
                return Ok(result);
            }
            result.Message = "Failed to decline friend request.";
            result.Success = false;
            result.Data = false;
            return BadRequest(result);
        }
    }
}
