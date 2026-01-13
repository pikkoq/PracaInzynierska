using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.UserDtos;
using ShoeBoardAPI.Services.UserService;
using Xunit;

namespace ShoeBoardAPITest
{
    public class UserServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().Options;
            _mockContext = new Mock<AppDbContext>(options);
            _mockConfiguration = new Mock<IConfiguration>();
            _mockMapper = new Mock<IMapper>();

            var userStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(
                userStore.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            _userService = new UserService(
                _mockContext.Object,
                _mockConfiguration.Object,
                _mockMapper.Object,
                _mockUserManager.Object,
                _mockSignInManager.Object);
        }

        [Fact]
        public async Task ChangeUserPassword_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var changePasswordDto = TestData.GetTestChangePasswordDto();
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.ChangeUserPassword(changePasswordDto, "nonexistent-user");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task ChangeUserPassword_ChangeFailed_ReturnsFalse()
        {
            // Arrange
            var changePasswordDto = TestData.GetTestChangePasswordDto();
            var user = TestData.GetTestUser1();
            
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockUserManager.Setup(x => x.ChangePasswordAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Failed" }));

            // Act
            var result = await _userService.ChangeUserPassword(changePasswordDto, user.Id);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Failed to change password", result.Message);
        }

        [Fact]
        public async Task EditUserData_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var editUserDto = TestData.GetTestEditUserDto();
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.EditUserData(editUserDto, "nonexistent-user");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task GetUser_UserNotFound_ReturnsFalse()
        {
            // Arrange
            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetUser("nonexistent-user");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task GetUser_UserExists_ReturnsUser()
        {
            // Arrange
            var user = TestData.GetTestUser1();
            var userDto = new GetUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Bio = user.Bio,
                ProfilePicturePath = user.ProfilePicturePath
            };

            _mockUserManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<GetUserDto>(It.IsAny<User>()))
                .Returns(userDto);

            // Act
            var result = await _userService.GetUser(user.Id);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(user.UserName, result.Data.Username);
        }

        [Fact]
        public async Task GetProfile_UserNotFound_ReturnsFalse()
        {
            // Arrange
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.GetProfile("nonexistent-user", "current-user");

            // Assert
            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task LoginUser_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var loginDto = TestData.GetTestLoginUserDto();
            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _userService.LoginUser(loginDto);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid username or password", result.Message);
        }

        [Fact]
        public async Task ReigsterUser_UsernameAlreadyTaken_ReturnsFalse()
        {
            // Arrange
            var registerDto = TestData.GetTestRegisterUserDto();
            var existingUser = TestData.GetTestUser1();

            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.ReigsterUser(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Username is already taken.", result.Message);
        }

        [Fact]
        public async Task ReigsterUser_EmailAlreadyInUse_ReturnsFalse()
        {
            // Arrange
            var registerDto = TestData.GetTestRegisterUserDto();
            var existingUser = TestData.GetTestUser1();

            _mockUserManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((User?)null);
            _mockUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _userService.ReigsterUser(registerDto);

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Email is already in use", result.Message);
        }

        [Fact]
        public async Task GenerateJwtToken_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = TestData.GetTestUser1();
            var roles = new List<string> { "User" };

            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("ThisIsAVeryLongSecretKeyForTesting12345678");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
            _mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<User>()))
                .ReturnsAsync(roles);

            // Act
            var result = await _userService.GenerateJwtToken(user);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void LoginUserDto_HasCorrectProperties()
        {
            // Arrange & Act
            var dto = TestData.GetTestLoginUserDto();

            // Assert
            Assert.Equal("TestUser1", dto.Login);
            Assert.Equal("TestPassword123!", dto.Password);
        }

        [Fact]
        public void RegisterUserDto_HasCorrectProperties()
        {
            // Arrange & Act
            var dto = TestData.GetTestRegisterUserDto();

            // Assert
            Assert.Equal("NewUser", dto.Username);
            Assert.Equal("newuser@example.com", dto.Email);
            Assert.Equal("NewPassword123!", dto.Password);
        }

        [Fact]
        public void User_DefaultValues_AreCorrect()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.True(user.DateCreated <= DateTime.UtcNow);
            Assert.NotNull(user.ProfilePicturePath);
            Assert.Equal(string.Empty, user.Bio);
        }
    }
}
