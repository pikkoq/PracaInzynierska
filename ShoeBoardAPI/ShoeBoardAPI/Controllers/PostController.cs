using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeBoardAPI.Models.DTO.PostDtos;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Services.PostService;
using System.Security.Claims;

namespace ShoeBoardAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("AddPost")]
        public async Task<IActionResult> AddPost([FromBody] CreatePostDto newPost)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<PostDto>
                {
                    Success = false,
                    Message = "User not found. Cannot identify. Please re-login."
                };
                return BadRequest(response);
            }

            var result = await _postService.AddPost(newPost, userId);
            if (result.Success)
            {
                result.Message = "Post added successfully!";
                return Ok(result);
            }

            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeletePost")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _postService.DeletePost(postId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetFriendPosts")]
        public async Task<IActionResult> GetFriendPosts(int pageNumber)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<List<PostDto>>
                {
                    Success = false,
                    Message = "User not found. Cannot identify."
                };
                return BadRequest(response);
            }

            var result = await _postService.GetPosts(userId, pageNumber);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetYoursPosts")]
        public async Task<IActionResult> GetYoursPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                var response = new ServiceResponse<List<PostDto>>
                {
                    Success = false,
                    Message = "User not found. Cannot identify."
                };
                return BadRequest(response);
            }

            var result = await _postService.GetYourPosts(userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentDto newComment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _postService.AddComment(newComment, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("DeleteComment")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _postService.DeleteComment(commentId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("LikePost")]
        public async Task<IActionResult> LikePost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _postService.LikePost(postId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("UnlikePost")]
        public async Task<IActionResult> UnlikePost(int postId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _postService.UnlikePost(postId, userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetComments")]
        public async Task<IActionResult> GetComments(int postId)
        {
            var result = await _postService.GetComments(postId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("GetLikeCount")]
        public async Task<IActionResult> GetLikeCount(int postId)
        {
            var result = await _postService.GetLikeCount(postId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
