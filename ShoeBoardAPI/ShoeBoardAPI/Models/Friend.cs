namespace ShoeBoardAPI.Models
{
    public class Friend
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }

        public User User { get; set; }
        public User FriendUser { get; set; }
    }
}
