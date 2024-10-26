using Microsoft.EntityFrameworkCore;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.PostDtos;

namespace ShoeBoardAPI.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<CommentDto>> AddComment(CreateCommentDto newComment, string userId)
        {
            var response = new ServiceResponse<CommentDto>();

            var post = await _context.Posts.FindAsync(newComment.PostId);
            if (post == null)
            {
                response.Success = false;
                response.Data = null;
                response.Message = "Post not found.";
                return response;
            }

            var comment = new Comment
            {
                PostId = newComment.PostId,
                UserId = userId,
                Content = newComment.Content,
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            response.Data = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserId = comment.UserId,
            };
            response.Message = "Added new comment.";
            response.Success = true;
            return response;
            
        }

        public async Task<ServiceResponse<PostDto>> AddPost(CreatePostDto newPost, string userId)
        {
            var response = new ServiceResponse<PostDto>();

            var shoeExists = await _context.Shoes.AnyAsync(sh => sh.UserId == userId && sh.Id == newPost.ShoeId);

            if (!shoeExists)
            {
                response.Success = false;
                response.Message = "Shoe not found in user's collection";
                response.Data = null;
                return response;
            }

            var post = new Post
            {
                ShoeId = newPost.ShoeId,
                UserId = userId,
                Content = newPost.Content,
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            response.Data = new PostDto
            {
                Id = post.Id,
                Content = post.Content,
                DatePosted = post.DatePosted,
                ShoeId = post.ShoeId,
                UserId = post.UserId,
                Comments = new List<CommentDto>(),
                LikeCount = 0
            };
            response.Success = true;
            response.Message = "New post created";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteComment(int commentId, string userId)
        {
            var response = new ServiceResponse<bool>();

            var comment = await _context.Comments.FindAsync(commentId);
            if(comment == null || comment.UserId != userId)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Comment not found or your are not the owener.";
                return response;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = "Comment deleted.";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeletePost(int postId, string userId)
        {
            var response = new ServiceResponse<bool>();

            var post = await _context.Posts.FindAsync(postId);

            if (post == null || post.UserId != userId)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Post not found or you are not the owenr.";
                return response;
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            response.Data = true;
            response.Success = true;
            response.Message = "Post deleted.";
            return response;
        }

        public async Task<ServiceResponse<List<CommentDto>>> GetComments(int postId)
        {
            var response = new ServiceResponse<List<CommentDto>>();

            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UserId = c.UserId,
                }).ToListAsync();

            response.Success = true;
            response.Data = comments;
            response.Message = "Successfully retrived data.";
            return response;
        }

        public async Task<ServiceResponse<int>> GetLikeCount(int postId)
        {
            var response = new ServiceResponse<int>();

            var likeCount = await _context.Likes.CountAsync(l => l.PostId == postId);
            response.Success = true;
            response.Data = likeCount;
            response.Message = "Retrived likes count.";
            return response;
        }

        public async Task<ServiceResponse<List<PostDto>>> GetPosts(string userId)
        {
            var response = new ServiceResponse<List<PostDto>>();

            var friendIds = await _context.Friends
                .Where(f => f.UserId == userId || f.FriendId == userId)
                .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                .ToListAsync();

            var posts = await _context.Posts
                .Where(p => friendIds.Contains(p.UserId))
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    DatePosted = p.DatePosted,
                    ShoeId = p.ShoeId,
                    UserId = p.UserId,
                    Comments = p.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        UserId = c.UserId,
                    }).ToList(),
                    LikeCount = p.Likes.Count,
                }).ToListAsync();

            response.Data = posts;
            response.Success = true;
            response.Message = "Successfully retrived post.";
            return response;
        }

        public async Task<ServiceResponse<List<PostDto>>> GetYourPosts(string userId)
        {
            var response = new ServiceResponse<List<PostDto>>();

            var posts = await _context.Posts
                .Where(p => p.UserId == userId)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Content = p.Content,
                    DatePosted = p.DatePosted,
                    ShoeId = p.ShoeId,
                    UserId = p.UserId,
                    Comments = p.Comments.Select(c => new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        UserId = c.UserId,
                    }).ToList(),
                    LikeCount = p.Likes.Count,
                }).ToListAsync();

            response.Data = posts;
            response.Success = true;
            response.Message = "Successfully retrived post.";
            return response;
        }

        public async Task<ServiceResponse<bool>> LikePost(int postId, string userId)
        {
            var response = new ServiceResponse<bool>();

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (like != null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "You have already liked this post.";
                return response;
            }

            _context.Likes.Add(new Like { PostId = postId, UserId = userId });
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = "Post liked.";
            return response;
        }

        public async Task<ServiceResponse<bool>> UnlikePost(int postId, string userId)
        {
            var response = new ServiceResponse<bool>();

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);
            if (like == null)
            {
                response.Success= false;
                response.Data = false;
                response.Message = "Like not found.";
                return response;
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Data = true;
            response.Message = "Unliked post.";
            return response;
        }
    }
}
