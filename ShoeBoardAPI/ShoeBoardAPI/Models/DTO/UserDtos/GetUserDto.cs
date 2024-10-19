namespace ShoeBoardAPI.Models.DTO.UserDtos
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
