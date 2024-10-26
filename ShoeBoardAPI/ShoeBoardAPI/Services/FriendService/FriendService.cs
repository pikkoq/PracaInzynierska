using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShoeBoardAPI.DataBase;
using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.FrindsDtos;

namespace ShoeBoardAPI.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FriendService(IMapper mapper, AppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<ServiceResponse<bool>> AcceptFriendRequest(string requestId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> DeclineFriendRequest(string requestId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> DeleteFriend(string userId, string friendId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<GetFriendRequestDto>>> GetFriendRequests(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<GetFriendsDto>>> GetFriends(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<bool>> SendFriendRequest(string requesterId, string receiverId)
        {
            var response = new ServiceResponse<bool>();

            var existingFriendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(r => r.RequesterId == requesterId && r.ReceiverId == receiverId);

            if (existingFriendRequest != null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Request already sent.";
                return response;
            }

            var existingReceiver = await _context.Users
                .FindAsync(receiverId);

            if (existingReceiver == null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "Incorrect user.";
                return response;
            }

            var friendRequest = new FriendRequest
            {
                RequesterId = requesterId,
                ReceiverId = receiverId,
                RequestDate = DateTime.UtcNow,
                IsAccepted = false,
            };

            _context.FriendRequests.Add(friendRequest);
            await _context.SaveChangesAsync();
            response.Success = true;
            response.Data = true;
            response.Message = "Friend request sent.";
            return response;
        }
    }
}
