using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.AdminDtos;
using ShoeBoardAPI.Services.AdminService.cs;
using Xunit;

namespace ShoeBoardAPITest
{
    public class AdminServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _adminService = new AdminService(_context, _mockMapper.Object);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task AccecptNewAddedShoes_ShoeNotFound_ReturnsFalse()
        {
            // Act
            var result = await _adminService.AccecptNewAddedShoes(999);

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Shoe not found.", result.Message);
        }

        [Fact]
        public async Task DeclineNewAddedShoes_ShoeNotFound_ReturnsFalse()
        {
            // Act
            var result = await _adminService.DeclineNewAddedShoes(999);

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Shoe not found.", result.Message);
        }

        [Fact]
        public async Task DeleteComment_CommentNotFound_ReturnsFalse()
        {
            // Act
            var result = await _adminService.DeleteComment(999);

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Comment not found.", result.Message);
        }

        [Fact]
        public async Task DeleteUserAccount_UserNotFound_ReturnsFalse()
        {
            // Act
            var result = await _adminService.DeleteUserAccount("nonexistent-user");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("User not found.", result.Message);
        }

        [Fact]
        public async Task DeleteUserPost_PostNotFound_ReturnsFalse()
        {
            // Act
            var result = await _adminService.DeleteUserPost(999);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Post not found.", result.Message);
        }

        [Fact]
        public async Task EditComment_CommentNotFound_ReturnsFalse()
        {
            // Act
            var result = await _adminService.EditComment(999, "New content");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Comment not found.", result.Message);
        }

        [Fact]
        public async Task EditNewAddedShoes_ShoeNotFound_ReturnsFalse()
        {
            // Arrange
            var editDto = TestData.GetTestEditNewAddedShoesDto();

            // Act
            var result = await _adminService.EditNewAddedShoes(999, editDto);

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Shoe not found.", result.Message);
        }

        [Fact]
        public async Task EditUserAccount_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var editDto = TestData.GetTestEditUserAccountDto();

            // Act
            var result = await _adminService.EditUserAccount("nonexistent-user", editDto);

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("User not found.", result.Message);
        }

        [Fact]
        public async Task EditUserPost_PostNotFound_ReturnsFalse()
        {
            // Act
            var result = await _adminService.EditUserPost(999, "New content");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Post not found.", result.Message);
        }

        [Fact]
        public async Task GetAllUsers_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            _mockMapper.Setup(m => m.Map<List<GetAllUsersDto>>(It.IsAny<List<User>>()))
                .Returns(new List<GetAllUsersDto>());

            // Act
            var result = await _adminService.GetAllUsers(1);

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetShoesToAccept_NoShoes_ReturnsEmptyList()
        {
            // Arrange
            _mockMapper.Setup(m => m.Map<List<GetShoesToAcceptDto>>(It.IsAny<List<UserShoeCatalog>>()))
                .Returns(new List<GetShoesToAcceptDto>());

            // Act
            var result = await _adminService.GetShoesToAccept(1);

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public void ServiceResponse_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var response = new ServiceResponse<bool>();

            // Assert
            Assert.True(response.Success);
            Assert.Equal(string.Empty, response.Message);
        }
    }
}
