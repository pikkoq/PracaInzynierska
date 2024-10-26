namespace ShoeBoardAPI.Models.DTO.FrindsDtos
{
    public class GetFriendsDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
