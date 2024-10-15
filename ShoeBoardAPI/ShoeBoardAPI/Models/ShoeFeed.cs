namespace ShoeBoardAPI.Models
{
    public class ShoeFeed
    {
        public int Id { get; set; }
        public int ShoeId { get; set; }
        public int FriendId { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        public Shoe Shoe { get; set; }
        public Friend Friend { get; set; }
    }
}
