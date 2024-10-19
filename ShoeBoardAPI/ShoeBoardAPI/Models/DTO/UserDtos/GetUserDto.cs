namespace ShoeBoardAPI.Models.DTO.UserDtos
{
    public class GetUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string ProfilePicturePath { get; set; }
        public string Bio { get; set; }
    }
}
