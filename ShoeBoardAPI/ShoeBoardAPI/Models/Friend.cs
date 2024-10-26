using System.ComponentModel.DataAnnotations;

namespace ShoeBoardAPI.Models
{
    public class Friend
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public DateTime DateAdded { get; set; }
        public User User { get; set; }
        public User FriendUser { get; set; }
    }
}
