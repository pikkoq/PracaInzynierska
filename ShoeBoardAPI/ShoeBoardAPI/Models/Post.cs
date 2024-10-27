namespace ShoeBoardAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int ShoeId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime DatePosted { get; set; } = DateTime.UtcNow;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public Shoe Shoe { get; set; }
        public User User { get; set; }
        
    }
}
