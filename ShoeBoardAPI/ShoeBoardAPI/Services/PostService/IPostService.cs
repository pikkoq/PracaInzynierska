using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.PostDtos;

namespace ShoeBoardAPI.Services.PostService
{
    public interface IPostService
    {
        Task<ServiceResponse<bool>> AddPost(CreatePostDto newPost, string userId);
        Task<ServiceResponse<bool>> DeletePost(int postId, string userId);
        Task<ServiceResponse<List<PostDto>>> GetPosts(string userId);
        Task<ServiceResponse<List<PostDto>>> GetYourPosts(string userId);
        Task<ServiceResponse<CommentDto>> AddComment(CreateCommentDto newComment, string userId);
        Task<ServiceResponse<bool>> DeleteComment(int commentId, string userId);
        Task<ServiceResponse<List<CommentDto>>> GetComments(int postId);
        Task<ServiceResponse<bool>> LikePost(int postId, string userId);
        Task<ServiceResponse<bool>> UnlikePost(int postId, string userId);
        Task<ServiceResponse<int>> GetLikeCount(int postId);
    }
}
