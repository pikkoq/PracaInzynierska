namespace ShoeBoardAPI.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }

        public User User { get; set; }
        public User FriendUser { get; set; }
    }
}
