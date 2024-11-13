using ShoeBoardAPI.Models;
using ShoeBoardAPI.Models.DTO.FrindsDtos;

namespace ShoeBoardAPI.Services.FriendService
{
    public interface IFriendService
    {
        Task<ServiceResponse<bool>> SendFriendRequest(string requesterId, string receiverId);
        Task<ServiceResponse<bool>> AcceptFriendRequest(int requestId, string userId);
        Task<ServiceResponse<bool>> DeclineFriendRequest(int requestId, string userId);
        Task<ServiceResponse<bool>> CancelSentFriendRequest(int requestId, string userId);
        Task<ServiceResponse<List<GetFriendRequestDto>>> GetFriendRequests(string userId);
        Task<ServiceResponse<List<GetFriendRequestDto>>> GetSentFriendRequests(string userId);
        Task<ServiceResponse<List<GetFriendsDto>>> GetFriends(string userId);
        Task<ServiceResponse<bool>> DeleteFriend(string userId, string friendId);
        Task<ServiceResponse<List<SearchFriendDto>>> SearchFriends(string searchTerm, string userId);
        
    }
}
