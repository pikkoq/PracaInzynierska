namespace ShoeBoardAPI.Models.DTO.UserDtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public bool Success { get; set; }
    }
}
