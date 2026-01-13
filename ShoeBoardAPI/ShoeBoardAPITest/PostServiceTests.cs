using Microsoft.EntityFrameworkCore;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.PostDtos;
using ShoeBoardAPI.Services.PostService;
using Xunit;

namespace ShoeBoardAPITest
{
    public class PostServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly PostService _postService;

        public PostServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _postService = new PostService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task AddComment_PostNotFound_ReturnsFalse()
        {
            // Arrange
            var newComment = TestData.GetTestCreateCommentDto();

            // Act
            var result = await _postService.AddComment(newComment, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Post not found.", result.Message);
        }

        [Fact]
        public async Task DeleteComment_CommentNotFound_ReturnsFalse()
        {
            // Act
            var result = await _postService.DeleteComment(999, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Comment not found or your are not the owener.", result.Message);
        }

        [Fact]
        public async Task DeleteComment_NotOwner_ReturnsFalse()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                PostId = 1,
                UserId = "other-user",
                Content = "Test comment"
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Act
            var result = await _postService.DeleteComment(1, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task DeletePost_PostNotFound_ReturnsFalse()
        {
            // Act
            var result = await _postService.DeletePost(999, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Post not found or you are not the owenr.", result.Message);
        }

        [Fact]
        public async Task DeletePost_NotOwner_ReturnsFalse()
        {
            // Arrange
            var post = new Post
            {
                Id = 1,
                ShoeId = 1,
                UserId = "other-user",
                Content = "Test post"
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var result = await _postService.DeletePost(1, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task GetComments_NoComments_ReturnsEmptyList()
        {
            // Act
            var result = await _postService.GetComments(999);

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetLikeCount_NoLikes_ReturnsZero()
        {
            // Act
            var result = await _postService.GetLikeCount(999);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(0, result.Data);
        }

        [Fact]
        public async Task GetYourPosts_NoPosts_ReturnsEmptyList()
        {
            // Act
            var result = await _postService.GetYourPosts("user-1");

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public void CreatePostDto_HasCorrectProperties()
        {
            // Arrange & Act
            var dto = TestData.GetTestCreatePostDto();

            // Assert
            Assert.Equal(1, dto.ShoeId);
            Assert.Equal("New post content", dto.Content);
        }

        [Fact]
        public void CreateCommentDto_HasCorrectProperties()
        {
            // Arrange & Act
            var dto = TestData.GetTestCreateCommentDto();

            // Assert
            Assert.Equal(1, dto.PostId);
            Assert.Equal("New comment", dto.Content);
        }

        [Fact]
        public void Post_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var post = new Post();

            // Assert
            Assert.NotNull(post.Comments);
            Assert.NotNull(post.Likes);
            Assert.Empty(post.Comments);
            Assert.Empty(post.Likes);
        }

        [Fact]
        public void Comment_DefaultCreatedAt_IsSet()
        {
            // Arrange & Act
            var comment = new Comment();

            // Assert
            Assert.True(comment.CreatedAt <= DateTime.UtcNow);
            Assert.True(comment.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
        }
    }
}
