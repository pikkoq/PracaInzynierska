using Microsoft.AspNetCore.Identity;

namespace ShoeBoardAPI.Models
{
    public class User : IdentityUser
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string? ProfilePicturePath { get; set; }
        public string? Bio {  get; set; }

        public ICollection<Shoe> Shoes { get; set; }
        public ICollection<Friend> Friends { get; set; }
        public ICollection<UserShoeCatalog> UserShoeCatalogs { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
