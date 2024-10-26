using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.FrindsDtos;

namespace ShoeBoardAPI.Services.FriendService
{
    public interface IFriendService
    {
        Task<ServiceResponse<bool>> SendFriendRequest(string requesterId, string receiverId);
        Task<ServiceResponse<bool>> AcceptFriendRequest(string requestId);
        Task<ServiceResponse<bool>> DeclineFriendRequest(string requestId);
        Task<ServiceResponse<List<GetFriendRequestDto>>> GetFriendRequests(string userId);
        Task<ServiceResponse<List<GetFriendsDto>>> GetFriends(string userId);
        Task<ServiceResponse<bool>> DeleteFriend(string userId, string friendId);

        
    }
}
