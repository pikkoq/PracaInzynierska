namespace ShoeBoardAPI.Models.DTO.UserDtos
{
    public class RegisterResponseDto
    {
        public string Message { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public bool Success { get; set; }
    }
}
