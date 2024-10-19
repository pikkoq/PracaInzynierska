namespace ShoeBoardAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string? ProilePicturePath { get; set; }
        public string? Bio {  get; set; }

        public ICollection<Shoe> Shoes { get; set; }
        public ICollection<Friend> Friends { get; set; }
    }
}
