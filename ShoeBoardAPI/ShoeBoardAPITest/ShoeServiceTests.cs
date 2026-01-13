using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.ShoeDtos;
using ShoeBoardAPI.Services.ShoeService;
using Xunit;

namespace ShoeBoardAPITest
{
    public class ShoeServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ShoeService _shoeService;

        public ShoeServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _shoeService = new ShoeService(_mockMapper.Object, _context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task DeleteUserShoe_ShoeNotFound_ReturnsFalse()
        {
            // Act
            var result = await _shoeService.DeleteUserShoe(999, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("User shoe not found.", result.Message);
        }

        [Fact]
        public async Task DeleteUserShoe_NotOwner_ReturnsFalse()
        {
            // Arrange
            var shoe = new Shoe
            {
                Id = 1,
                UserId = "other-user",
                ShoeCatalogId = 1,
                Size = "42",
                Review = "Test review",
                Season = "Summer"
            };
            _context.Shoes.Add(shoe);
            await _context.SaveChangesAsync();

            // Act
            var result = await _shoeService.DeleteUserShoe(1, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("You can delete only yours shoes!", result.Message);
        }

        [Fact]
        public async Task EditUserShoe_ShoeNotFound_ReturnsFalse()
        {
            // Arrange
            var editDto = TestData.GetTestEditUserShoeDto();

            // Act
            var result = await _shoeService.EditUserShoe(999, editDto, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Shoe not found.", result.Message);
        }

        [Fact]
        public async Task EditUserShoe_NotOwner_ReturnsFalse()
        {
            // Arrange
            var editDto = TestData.GetTestEditUserShoeDto();
            var shoe = new Shoe
            {
                Id = 1,
                UserId = "other-user",
                ShoeCatalogId = 1,
                Size = "42",
                Review = "Test review",
                Season = "Summer"
            };
            _context.Shoes.Add(shoe);
            await _context.SaveChangesAsync();

            // Act
            var result = await _shoeService.EditUserShoe(1, editDto, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("You can edit only yours shoes!", result.Message);
        }

        [Fact]
        public async Task GetAllUserShoes_NoShoes_ReturnsEmptyList()
        {
            // Act
            var result = await _shoeService.GetAllUserShoes("user-1");

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetCatalogShoeDetails_ShoeNotFound_ReturnsFalse()
        {
            // Act
            var result = await _shoeService.GetCatalogShoeDetails(999);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Failed to retrive details data", result.Message);
        }

        [Fact]
        public async Task GetShoeDetails_ShoeNotFound_ReturnsFalse()
        {
            // Act
            var result = await _shoeService.GetShoeDetails(999);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Data);
            Assert.Equal("Shoe not found", result.Message);
        }

        [Fact]
        public async Task GetTopPoulrShoes_NoShoes_ReturnsEmptyList()
        {
            // Act
            var result = await _shoeService.GetTopPoulrShoes();

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task NewShoeRegistry_MissingTitle_ReturnsFalse()
        {
            // Arrange
            var newShoeDto = new NewShoeRegistryDto();
            var emptyUserShoeCatalog = new UserShoeCatalog { Title = "" };
            _mockMapper.Setup(m => m.Map<UserShoeCatalog>(It.IsAny<NewShoeRegistryDto>()))
                .Returns(emptyUserShoeCatalog);

            // Act
            var result = await _shoeService.NewShoeRegistry(newShoeDto, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.Contains("can't be empty", result.Message);
        }

        [Fact]
        public async Task SignShoeToUser_ValidData_ReturnsTrue()
        {
            // Arrange
            var signShoeDto = TestData.GetTestSignShoeToUserDto();

            // Act
            var result = await _shoeService.SignShoeToUser(signShoeDto, 1, "user-1");

            // Assert
            Assert.True(result.Success);
            Assert.True(result.Data);
            Assert.Equal("Shoe successfully added to user's collection.", result.Message);
        }

        [Fact]
        public void EditUserShoeDto_HasCorrectProperties()
        {
            // Arrange & Act
            var dto = TestData.GetTestEditUserShoeDto();

            // Assert
            Assert.Equal("43", dto.Size);
            Assert.Equal(4, dto.ComfortRating);
            Assert.Equal(5, dto.StyleRating);
            Assert.Equal("Winter", dto.Season);
            Assert.Equal("Updated review", dto.Review);
        }

        [Fact]
        public void SignShoeToUserDto_HasCorrectProperties()
        {
            // Arrange & Act
            var dto = TestData.GetTestSignShoeToUserDto();

            // Assert
            Assert.Equal("42", dto.Size);
            Assert.Equal(5, dto.ComfortRating);
            Assert.Equal(4, dto.StyleRating);
            Assert.Equal("Summer", dto.Season);
            Assert.Equal("Great fit!", dto.Review);
        }

        [Fact]
        public void Shoe_DefaultDateAdded_IsSet()
        {
            // Arrange & Act
            var shoe = new Shoe();

            // Assert
            Assert.True(shoe.DateAdded <= DateTime.UtcNow);
            Assert.True(shoe.DateAdded > DateTime.UtcNow.AddMinutes(-1));
        }
    }
}
