using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShoeBoardAPI.Models
{
    public class User : IdentityUser
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string ProfilePicturePath { get; set; } = "https://icons.veryicon.com/png/o/miscellaneous/common-icons-31/default-avatar-2.png";
        [MaxLength(500)]
        public string? Bio {  get; set; } = string.Empty;

        public ICollection<Shoe> Shoes { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<UserShoeCatalog> UserShoeCatalogs { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
