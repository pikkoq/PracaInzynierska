using System.ComponentModel.DataAnnotations;

namespace ShoeBoardAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        [MaxLength(100)]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Post Post { get; set; }
        public User User { get; set; }

    }
}
