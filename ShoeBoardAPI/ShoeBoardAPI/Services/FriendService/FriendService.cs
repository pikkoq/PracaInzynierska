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

        public async Task<ServiceResponse<bool>> AcceptFriendRequest(int requestId, string userId)
        {
            var response = new ServiceResponse<bool>();

            var friendRequest = await _context.FriendRequests.FindAsync(requestId);
            if (friendRequest == null || friendRequest.IsAccepted || friendRequest.ReceiverId != userId)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "Faield to accept request.";
                return response;
            }
            friendRequest.IsAccepted = true;
            _context.Friends.Add(new Friend
            {
                UserId = friendRequest.ReceiverId,
                FriendId = friendRequest.RequesterId,
                DateAdded = DateTime.UtcNow
            });
            _context.FriendRequests.Remove(friendRequest);
            await _context.SaveChangesAsync();
            
            response.Data = true;
            response.Success = true;
            response.Message = "Friend accepted";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeclineFriendRequest(int requestId, string userId)
        {
            var response = new ServiceResponse<bool>();

            var friendRequest = await _context.FriendRequests.FindAsync(requestId);
            if (friendRequest == null || friendRequest.ReceiverId != userId)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "Failed to decline friend request.";
                return response;
            }

            _context.FriendRequests.Remove(friendRequest);
            await _context.SaveChangesAsync();

            response.Data = true;
            response.Success = true;
            response.Message = "Friend request declined.";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteFriend(string userId, string friendId)
        {
            var response = new ServiceResponse<bool>();

            var friendship = await _context.Friends
                .FirstOrDefaultAsync(f =>
                (f.UserId == userId && f.FriendId == friendId) ||
                (f.UserId == friendId && f.FriendId == userId));

            if (friendship == null)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "Friendship not found";
                return response;
            }

            _context.Friends.Remove(friendship);
            await _context.SaveChangesAsync();

            response.Data = true;
            response.Success = true;
            response.Message = "Friend removed.";
            return response;
        }

        public async Task<ServiceResponse<List<GetFriendRequestDto>>> GetFriendRequests(string userId)
        {
            var response = new ServiceResponse<List<GetFriendRequestDto>>();

            var friends = await _context.FriendRequests
                .Where(f => f.ReceiverId == userId && f.IsAccepted == false)
                .Include(f => f.Requester)
                .Select(f => new GetFriendRequestDto
                {
                    Username = f.Requester.UserName,
                    RequestDate = f.RequestDate,
                    IsAccepted = f.IsAccepted,
                })
                .ToListAsync();

            response.Success = true;
            response.Message = "Successfully retrived data.";
            response.Data = friends;
            return response;
        }
        public async Task<ServiceResponse<List<GetFriendRequestDto>>> GetSentFriendRequests(string userId)
        {
            var response = new ServiceResponse<List<GetFriendRequestDto>>();

            var friends = await _context.FriendRequests
                .Where(f => f.RequesterId == userId && f.IsAccepted == false)
                .Include(f => f.Receiver)
                .Select(f => new GetFriendRequestDto
                {
                    Username = f.Receiver.UserName,
                    RequestDate = f.RequestDate,
                    IsAccepted = f.IsAccepted,
                })
                .ToListAsync();

            response.Success = true;
            response.Message = "Successfully retrived data.";
            response.Data = friends;
            return response;
        }

        public async Task<ServiceResponse<List<GetFriendsDto>>> GetFriends(string userId)
        {
            var response = new ServiceResponse<List<GetFriendsDto>>();

            var friends = await _context.Friends
                .Where(f => f.UserId == userId || f.FriendId == userId)
                .Include(f => f.FriendUser)
                .Include(f => f.User)
                .Select(f => new GetFriendsDto
                {
                    Username = userId == f.UserId ? f.FriendUser.UserName : f.User.UserName,
                    DateAdded = f.DateAdded,
                })
                .ToListAsync();

            response.Success = true;
            response.Message = "Successfully retrived data.";
            response.Data = friends;
            return response;
        }

        public async Task<ServiceResponse<bool>> SendFriendRequest(string requesterId, string receiverId)
        {
            var response = new ServiceResponse<bool>();

            if (requesterId == receiverId)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "You cannot send a friend request to yourself.";
                return response;
            }

            var existingFriendship = await _context.Friends
                .FirstOrDefaultAsync(f =>
                (f.UserId == requesterId && f.FriendId == receiverId) ||
                (f.UserId == receiverId && f.FriendId == requesterId));

            if (existingFriendship != null)
            {
                response.Success = false;
                response.Data = false;
                response.Message = "You are already friends.";
                return response;
            }

            var existingFriendRequest = await _context.FriendRequests
                .FirstOrDefaultAsync(r =>
                (r.RequesterId == requesterId && r.ReceiverId == receiverId) ||
                (r.RequesterId == receiverId && r.ReceiverId == requesterId));

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
