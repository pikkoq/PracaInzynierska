using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Services.FriendService;
using Xunit;

namespace ShoeBoardAPITest
{
    public class FriendServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly FriendService _friendService;

        public FriendServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _friendService = new FriendService(_mockMapper.Object, _context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task AcceptFriendRequest_RequestNotFound_ReturnsFalse()
        {
            // Act
            var result = await _friendService.AcceptFriendRequest(999, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Faield to accept request.", result.Message);
        }

        [Fact]
        public async Task DeclineFriendRequest_RequestNotFound_ReturnsFalse()
        {
            // Act
            var result = await _friendService.DeclineFriendRequest(999, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Failed to decline friend request.", result.Message);
        }

        [Fact]
        public async Task CancelSentFriendRequest_RequestNotFound_ReturnsFalse()
        {
            // Act
            var result = await _friendService.CancelSentFriendRequest(999, "user-1");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("Failed to cancel friend request.", result.Message);
        }

        [Fact]
        public async Task SendFriendRequest_ToSelf_ReturnsFalse()
        {
            // Arrange
            var userId = "user-1";

            // Act
            var result = await _friendService.SendFriendRequest(userId, userId);

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
            Assert.Equal("You cannot send a friend request to yourself.", result.Message);
        }

        [Fact]
        public async Task AcceptFriendRequest_AlreadyAccepted_ReturnsFalse()
        {
            // Arrange
            var acceptedRequest = new FriendRequest
            {
                Id = 1,
                RequesterId = "user-1",
                ReceiverId = "user-2",
                IsAccepted = true
            };
            _context.FriendRequests.Add(acceptedRequest);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendService.AcceptFriendRequest(1, "user-2");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task AcceptFriendRequest_WrongReceiver_ReturnsFalse()
        {
            // Arrange
            var request = new FriendRequest
            {
                Id = 1,
                RequesterId = "user-1",
                ReceiverId = "user-2",
                IsAccepted = false
            };
            _context.FriendRequests.Add(request);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendService.AcceptFriendRequest(1, "user-3");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task DeclineFriendRequest_WrongReceiver_ReturnsFalse()
        {
            // Arrange
            var request = new FriendRequest
            {
                Id = 1,
                RequesterId = "user-1",
                ReceiverId = "user-2",
                IsAccepted = false
            };
            _context.FriendRequests.Add(request);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendService.DeclineFriendRequest(1, "user-3");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task CancelSentFriendRequest_WrongRequester_ReturnsFalse()
        {
            // Arrange
            var request = new FriendRequest
            {
                Id = 1,
                RequesterId = "user-1",
                ReceiverId = "user-2",
                IsAccepted = false
            };
            _context.FriendRequests.Add(request);
            await _context.SaveChangesAsync();

            // Act
            var result = await _friendService.CancelSentFriendRequest(1, "user-2");

            // Assert
            Assert.False(result.Success);
            Assert.False(result.Data);
        }

        [Fact]
        public async Task GetFriendRequests_NoRequests_ReturnsEmptyList()
        {
            // Act
            var result = await _friendService.GetFriendRequests("user-1");

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetSentFriendRequests_NoRequests_ReturnsEmptyList()
        {
            // Act
            var result = await _friendService.GetSentFriendRequests("user-1");

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetFriends_NoFriends_ReturnsEmptyList()
        {
            // Act
            var result = await _friendService.GetFriends("user-1");

            // Assert
            Assert.True(result.Success);
            Assert.Empty(result.Data);
        }
    }
}
