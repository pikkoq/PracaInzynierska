namespace ShoeBoardAPI.Models
{
    public class ShoeFeed
    {
        public string Id { get; set; }
        public string ShoeId { get; set; }
        public string FriendId { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        public Shoe Shoe { get; set; }
        public Friend Friend { get; set; }
    }
}
