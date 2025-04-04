﻿using Microsoft.EntityFrameworkCore;
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
            };
            response.Message = "Added new comment.";
            response.Success = true;
            return response;
            
        }

        public async Task<ServiceResponse<bool>> AddPost(CreatePostDto newPost, string userId)
        {
            var response = new ServiceResponse<bool>();

            var shoeExists = await _context.Shoes.AnyAsync(sh => sh.UserId == userId && sh.Id == newPost.ShoeId);

            if (!shoeExists)
            {
                response.Success = false;
                response.Message = "Shoe not found in user's collection";
                response.Data = false;
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

            response.Data = true;
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
                .Include(c => c.User)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    Username = c.User.UserName,
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

        public async Task<ServiceResponse<List<PostDto>>> GetPosts(string userId, int pageNumber = 1)
        {
            var response = new ServiceResponse<List<PostDto>>();

            int pageSize = 10;

            var friendIds = await _context.Friends
                .Where(f => f.UserId == userId || f.FriendId == userId)
                .Select(f => f.UserId == userId ? f.FriendId : f.UserId)
                .ToListAsync();

            var query = _context.Posts
                .Where(p => friendIds.Contains(p.UserId))
                .Include(p => p.Shoe)
                    .ThenInclude(s => s.ShoeCatalog)
                .OrderByDescending(p => p.DatePosted)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    ShoeCatalogId = p.Shoe.ShoeCatalogId,
                    Username = p.User.UserName,
                    ProfilePictureUrl = p.User.ProfilePicturePath,
                    Content = p.Content,
                    DatePosted = p.DatePosted,
                    Size = p.Shoe.Size,
                    ComfortRating = p.Shoe.ComfortRating,
                    StyleRating = p.Shoe.StyleRating,
                    Season = p.Shoe.Season,
                    Review = p.Shoe.Review,
                    Image_Url = p.Shoe.ShoeCatalog.Image_Url,
                    Title = p.Shoe.ShoeCatalog.Title,
                    IsLiked = p.Likes.Any(l => l.UserId == userId),
                    LikeCount = p.Likes.Count,
                    CommentsCount = p.Comments.Count,
                });

            var posts = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
                .Include(p => p.Shoe)
                    .ThenInclude(s => s.ShoeCatalog)
                .OrderByDescending(o => o.DatePosted)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    Username = p.User.UserName,
                    Content = p.Content,
                    DatePosted = p.DatePosted,
                    Size = p.Shoe.Size,
                    ComfortRating = p.Shoe.ComfortRating,
                    StyleRating = p.Shoe.StyleRating,
                    Season = p.Shoe.Season,
                    Review = p.Shoe.Review,
                    Image_Url = p.Shoe.ShoeCatalog.Image_Url,
                    Title = p.Shoe.ShoeCatalog.Title,
                    IsLiked = p.Likes.Any(l => l.UserId == userId),
                    CommentsCount = p.Comments.Count,
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
